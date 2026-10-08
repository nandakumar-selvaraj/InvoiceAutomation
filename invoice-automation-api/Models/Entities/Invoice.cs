namespace InvoiceAutomationApi.Models.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Customer { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? Currency { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }

        // Relationship: One Invoice has many Items
        public List<InvoiceDetail> Items { get; set; } = new();
    }
}
