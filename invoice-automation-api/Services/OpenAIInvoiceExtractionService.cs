#pragma warning disable OPENAI001

using System.Text.Json;
using InvoiceAutomationApi.Models.Dtos;
using OpenAI.Responses;

namespace InvoiceAutomationApi.Services
{
    public class OpenAIInvoiceExtractionService : IInvoiceExtractionService
    {
        private readonly string _apiKey;
        private readonly string _model;
        public OpenAIInvoiceExtractionService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new InvalidOperationException(
                    "OpenAI API key is not configured.");
            _model = configuration["OpenAI:Model"]
                ?? throw new InvalidOperationException(
                    "OpenAI model is not configured.");
        }

        public async Task<InvoiceResponse> ExtractInvoiceAsync(IFormFile file)
        {
            // Read PDF into memory
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            BinaryData pdfData =
                BinaryData.FromBytes(memoryStream.ToArray());

            // Create OpenAI client
            ResponsesClient client = new(_apiKey);

            // Instruction for invoice extraction
            string prompt = GetPrompt();

            // Create the request
            CreateResponseOptions options = GetResponseOptions(_model, pdfData, prompt, file.FileName);

            // Send PDF to OpenAI
            ResponseResult response =
                await client.CreateResponseAsync(options);

            // Get AI output
            string json = response.GetOutputText();

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException(
                    "OpenAI returned an empty response.");
            }

            // Convert JSON into our InvoiceResponse model
            InvoiceResponse? invoice =
                JsonSerializer.Deserialize<InvoiceResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (invoice == null)
            {
                throw new InvalidOperationException(
                    "Unable to parse the invoice data returned by OpenAI.");
            }

            return invoice;
        }


        private static string GetPrompt()
        {
            return """
        You are an invoice data extraction system. Analyze the provided PDF and extract the invoice information following these strict guidelines:

        PAGE & TABLE BOUNDARIES:
        - Read and process ONLY the 1st page of the PDF document. Ignore all subsequent pages.
        - Extract line items strictly from the 1st/main product table on Page 1. Ignore secondary tables, payment summaries, or marketplace fee tables.

        FIELD DEFINITIONS:
        1. InvoiceNumber
        - Extract the value explicitly labeled "Invoice Number".
        - Do not use the Order Number.
        - Preserve the invoice number exactly as shown.

        2. Customer
        - Use the customer/recipient name shown under "Billing Address".
        - Do NOT use the seller name.
        - Do NOT use the "Sold By" name.
        - Do NOT use the authorized signatory name.
        - If a Billing Address is present, use the person's or company's name shown there.

        3. InvoiceDate
        - Extract the value explicitly labeled "Invoice Date".
        - Return it in YYYY-MM-DD format.

        4. Currency
        - Identify the currency used for the invoice amounts.
        - For Indian Rupee (₹), return "INR".

        5. Items
        - Extract every actual product/service line item from the 1st table on Page 1.
        - Do NOT treat Shipping Charges as a product item.
        - Ignore OCR noise/artifacts in tax columns (such as non-Latin/Devanagari characters or prefix errors like R0.00 or २०.००).
        - For each item extract:
          * Description
          * Quantity
          * UnitPrice
          * Amount

        6. Subtotal
        - Extract the invoice subtotal if explicitly available.
        - Do not confuse subtotal with total.
        - If not explicitly labeled as Subtotal, return null.

        7. Tax
        - Extract the total tax amount explicitly shown for the invoice.
        - If multiple taxes such as CGST and SGST are shown, use their combined total when a total tax amount is explicitly shown.

        8. Total
        - Extract the final invoice total amount.

        RULES:
        - Extract only information that is actually present in Page 1 of the invoice.
        - Do not guess or invent values.
        - If a field cannot be determined, return null.
        - Preserve invoice numbers exactly as shown.
        - Return monetary values as numbers without currency symbols or commas (e.g., 775.00).
        - Return quantity as a number.
        - Return the invoice date as YYYY-MM-DD.
        - Return ONLY valid JSON matching this schema:

        {
          "InvoiceNumber": "string",
          "Customer": "string",
          "InvoiceDate": "YYYY-MM-DD",
          "Currency": "string",
          "Items": [
            {
              "Description": "string",
              "Quantity": 1,
              "UnitPrice": 0.00,
              "Amount": 0.00
            }
          ],
          "Subtotal": null,
          "Tax": 0.00,
          "Total": 0.00
        }
        """;
        }

        private static CreateResponseOptions GetResponseOptions(string model, BinaryData pdfData, string prompt, string fileName)
        {
            CreateResponseOptions options = new()
            {
                Model = model
            };
            options.InputItems.Add(
                ResponseItem.CreateUserMessageItem(
                    [
                        ResponseContentPart.CreateInputFilePart(
                            pdfData,
                            "application/pdf",
                            fileName),
                        ResponseContentPart.CreateInputTextPart(prompt)
                    ]
                )
            );
            return options;
        }
    }
}

#pragma warning restore OPENAI001