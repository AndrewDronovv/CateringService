﻿namespace CateringService.Application.DataTransferObjects.Responses;

public sealed class TenantViewModel
{
    public Ulid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? BlockReason { get; set; } = string.Empty;
}