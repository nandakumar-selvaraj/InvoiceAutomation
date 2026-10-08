using InvoiceAutomationApi.Models.Dtos;
using InvoiceAutomationApi.Models.Entities;

namespace InvoiceAutomationApi.Models
{
    public static class MappingExtensions
    {
        // Convert DTO received from API into a Database Entity
        public static Invoice ToEntity(this InvoiceResponse dto)
        {
            return new Invoice
            {
                InvoiceNumber = dto.InvoiceNumber,
                Customer = dto.Customer,
                Currency = dto.Currency,
                Subtotal = dto.Subtotal,
                Tax = dto.Tax,
                Total = dto.Total,
                // Parse string date safely into DateTime? for SQLite
                InvoiceDate = DateTime.TryParse(dto.InvoiceDate, out var date) ? date : null,

                Items = dto.Items.Select(item => new InvoiceDetail
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Amount = item.Amount
                }).ToList()
            };
        }

        // Convert Database Entity into a DTO to send back via API
        public static InvoiceResponse ToDto(this Invoice entity)
        {
            return new InvoiceResponse
            {
                InvoiceNumber = entity.InvoiceNumber,
                Customer = entity.Customer,
                Currency = entity.Currency,
                Subtotal = entity.Subtotal,
                Tax = entity.Tax,
                Total = entity.Total,
                InvoiceDate = entity.InvoiceDate?.ToString("yyyy-MM-dd"),

                Items = entity.Items.Select(item => new InvoiceItem
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Amount = item.Amount
                }).ToList()
            };
        }
    }
}
