using ClosedXML.Excel;
using InvoiceAutomationApi.Models;
using InvoiceAutomationApi.Models.Dtos;
using InvoiceAutomationApi.Models.Entities;
using InvoiceAutomationApi.Repositories;

namespace InvoiceAutomationApi.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;

        public InvoiceService(IInvoiceRepository repo) => _repo = repo;

        public async Task<InvoiceResponse?> GetInvoiceAsync(string invoiceNumber)
        {
            var invoice = await _repo.GetByInvoiceNumberAsync(invoiceNumber);
            return invoice?.ToDto();
        }

        public async Task<IEnumerable<InvoiceResponse>> GetAllInvoicesAsync()
        {
            var invoices = await _repo.GetAllAsync();
            return invoices.Select(i => i.ToDto());
        }

        public async Task<InvoiceResponse> CreateInvoiceAsync(InvoiceResponse dto)
        {
            var entity = dto.ToEntity();
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity.ToDto();
        }

        public async Task<bool> UpdateInvoiceAsync(string invoiceNumber, InvoiceResponse dto)
        {
            var existingInvoice = await _repo.GetByInvoiceNumberAsync(invoiceNumber);
            if (existingInvoice == null) return false;

            // Map updated scalar properties
            existingInvoice.Customer = dto.Customer;
            existingInvoice.Currency = dto.Currency;
            existingInvoice.Subtotal = dto.Subtotal;
            existingInvoice.Tax = dto.Tax;
            existingInvoice.Total = dto.Total;
            existingInvoice.InvoiceDate = DateTime.TryParse(dto.InvoiceDate, out var date) ? date : null;

            // Clear and reload relationships safely
            _repo.DeleteItemsRange(existingInvoice.Items);
            existingInvoice.Items = dto.Items.Select(item => new InvoiceDetail
            {
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Amount = item.Amount
            }).ToList();

            _repo.Update(existingInvoice);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteInvoiceAsync(string invoiceNumber)
        {
            var invoice = await _repo.GetByInvoiceNumberAsync(invoiceNumber);
            if (invoice == null) return false;

            _repo.Delete(invoice);
            return await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<InvoiceResponse>> CreateInvoicesBulkAsync(IEnumerable<InvoiceResponse> dtos)
        {
            if (dtos == null || !dtos.Any()) return Enumerable.Empty<InvoiceResponse>();

            // 1. Map all incoming DTOs into Entity formats
            var entities = dtos.Select(dto => dto.ToEntity()).ToList();

            // 2. Pass the list to the repository layer
            foreach (var entity in entities)
            {
                await _repo.AddAsync(entity);
            }

            // 3. Save everything to SQLite in a single transaction database call
            await _repo.SaveChangesAsync();

            // 4. Return the newly created records mapped back to DTOs
            return entities.Select(e => e.ToDto());
        }

        public async Task<byte[]> ExportInvoicesToExcelAsync()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Extracted Invoices");

            // Styling Configurations matching UI Color Palette
            var headerBackground = XLColor.FromHtml("#2D3748"); // Dark Slate/Navy Header
            var lineItemHeaderBg = XLColor.FromHtml("#4A5568"); // Lighter Gray for line items
            var keyBlueColor = XLColor.FromHtml("#2B6CB0");     // Link Blue for Invoice No.
            var textWhite = XLColor.White;

            int currentRow = 1;
            var invoices = await _repo.GetAllAsync();
            foreach (var invoice in invoices)
            {
                // --- 1. Parent Invoice Table Header ---
                var parentHeaders = new[] { "Invoice No.", "Customer", "Date", "Subtotal", "Tax", "Total" };
                for (int i = 0; i < parentHeaders.Length; i++)
                {
                    var cell = worksheet.Cell(currentRow, i + 1);
                    cell.Value = parentHeaders[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = headerBackground;
                    cell.Style.Font.FontColor = textWhite;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
                currentRow++;

                // --- 2. Parent Invoice Data Row ---
                worksheet.Cell(currentRow, 1).Value = invoice.InvoiceNumber;
                worksheet.Cell(currentRow, 1).Style.Font.FontColor = keyBlueColor;
                worksheet.Cell(currentRow, 1).Style.Font.Underline = XLFontUnderlineValues.Single; // Visual Anchor link style

                worksheet.Cell(currentRow, 2).Value = invoice.Customer;
                worksheet.Cell(currentRow, 3).Value = invoice.InvoiceDate?.ToString("yyyy-MM-dd");

                worksheet.Cell(currentRow, 4).Value = invoice.Subtotal;
                worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "₹#,##0.00";

                worksheet.Cell(currentRow, 5).Value = invoice.Tax;
                worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "₹#,##0.00";

                worksheet.Cell(currentRow, 6).Value = invoice.Total;
                worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "₹#,##0.00";

                // Border layout for the parent invoice record block
                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(currentRow, 1, currentRow, 6).Style.Border.OutsideBorderColor = XLColor.LightGray;
                currentRow += 2; // Leave a slight gap for line items block

                // --- 3. Child Line Items Section ---
                worksheet.Cell(currentRow, 1).Value = "LINE ITEMS DETAIL";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Range(currentRow, 1, currentRow, 5).Merge(); // Span across
                currentRow++;

                // Line Item Table Headers
                var itemHeaders = new[] { "Description", "Quantity", "Unit Price", "Amount" };
                for (int i = 0; i < itemHeaders.Length; i++)
                {
                    var cell = worksheet.Cell(currentRow, i + 1);
                    cell.Value = itemHeaders[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = lineItemHeaderBg;
                    cell.Style.Font.FontColor = textWhite;
                }
                currentRow++;

                // Populate Line Items rows
                foreach (var item in invoice.Items)
                {
                    worksheet.Cell(currentRow, 1).Value = item.Description;
                    worksheet.Cell(currentRow, 1).Style.Alignment.WrapText = true; // Auto wrap long text descriptions

                    worksheet.Cell(currentRow, 2).Value = item.Quantity;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(currentRow, 3).Value = item.UnitPrice;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "₹#,##0.00";

                    worksheet.Cell(currentRow, 4).Value = item.Amount;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "₹#,##0.00";

                    // Thin grid borders matching screenshot aesthetic
                    worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    worksheet.Range(currentRow, 1, currentRow, 4).Style.Border.BottomBorderColor = XLColor.LightGray;
                    currentRow++;
                }

                currentRow += 3; // Section spacer layout before starting the next invoice mapping segment
            }

            // Auto-fit grid sizes dynamically so text does not truncate
            worksheet.Columns(1, 6).AdjustToContents();
            worksheet.Column(1).Width = 40; // Hard cap description width layout to preserve viewability

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
