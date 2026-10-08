using InvoiceAutomationApi.Models;
using InvoiceAutomationApi.Models.Dtos;
using InvoiceAutomationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAutomationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceExtractionService _invoiceExtractionService;

        private readonly IInvoiceService _invoiceService;

        public InvoicesController(
            IInvoiceExtractionService invoiceExtractionService,
            IInvoiceService invoiceService)
        {
            _invoiceExtractionService = invoiceExtractionService;
            _invoiceService = invoiceService;
        }

        [HttpPost("extract")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ExtractInvoice(
            IFormFile file)
        {
            // Validate file
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Please upload an invoice PDF."
                });
            }

            // Validate PDF
            if (!string.Equals(
                    file.ContentType,
                    "application/pdf",
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Only PDF files are supported."
                });
            }

            try
            {
                // Send invoice to extraction service
                var result =
                    await _invoiceExtractionService
                        .ExtractInvoiceAsync(file);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Invoice extraction failed.",
                    error = ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<ActionResult<InvoiceResponse>> Create([FromBody] InvoiceResponse request)
        {
            var result = await _invoiceService.CreateInvoiceAsync(request);
            return CreatedAtAction(nameof(Get), new { invoiceNumber = result.InvoiceNumber }, result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetAll()
        {
            return Ok(await _invoiceService.GetAllInvoicesAsync());
        }

        [HttpGet("{invoiceNumber}")]
        public async Task<ActionResult<InvoiceResponse>> Get(string invoiceNumber)
        {
            var result = await _invoiceService.GetInvoiceAsync(invoiceNumber);
            if (result == null) return NotFound($"Invoice {invoiceNumber} not found.");
            return Ok(result);
        }

        [HttpPut("{invoiceNumber}")]
        public async Task<IActionResult> Update(string invoiceNumber, [FromBody] InvoiceResponse request)
        {
            var success = await _invoiceService.UpdateInvoiceAsync(invoiceNumber, request);
            if (!success) return NotFound($"Invoice {invoiceNumber} not found.");
            return NoContent();
        }

        [HttpDelete("{invoiceNumber}")]
        public async Task<IActionResult> Delete(string invoiceNumber)
        {
            var success = await _invoiceService.DeleteInvoiceAsync(invoiceNumber);
            if (!success) return NotFound($"Invoice {invoiceNumber} not found.");
            return NoContent();
        }

        // POST: api/invoices/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<InvoiceResponse>>> CreateBulk([FromBody] IEnumerable<InvoiceResponse> requests)
        {
            if (requests == null || !requests.Any())
            {
                return BadRequest("The invoice collection cannot be empty.");
            }

            var results = await _invoiceService.CreateInvoicesBulkAsync(requests);
            return Ok(results);
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            byte[] excelBytes = await _invoiceService.ExportInvoicesToExcelAsync();

            string fileName = $"Invoices_Export_{DateTime.Now:yyyyMMdd}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(excelBytes, contentType, fileName);
        }

    }
}