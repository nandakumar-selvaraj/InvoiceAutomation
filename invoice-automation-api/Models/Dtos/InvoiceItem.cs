namespace InvoiceAutomationApi.Models.Dtos
{
    public class InvoiceItem
    {
        public string? Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Amount { get; set; }
    }
}