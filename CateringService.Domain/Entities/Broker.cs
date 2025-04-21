using CateringService.Domain.Common;

namespace CateringService.Domain.Entities;

public sealed class Broker : UlidEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;

    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
}