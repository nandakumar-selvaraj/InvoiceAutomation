namespace InvoiceAutomationApi.Models.Dtos
{
    public class InvoiceResponse
    {
        public string? InvoiceNumber { get; set; }

        public string? Customer { get; set; }

        public string? InvoiceDate { get; set; }

        public string? Currency { get; set; }

        public List<InvoiceItem> Items { get; set; } = new();

        public decimal? Subtotal { get; set; }

        public decimal? Tax { get; set; }

        public decimal? Total { get; set; }
    }
}