namespace InvoiceAutomationApi.Models.Entities
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }

        // Foreign Key mapping back to parent
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
