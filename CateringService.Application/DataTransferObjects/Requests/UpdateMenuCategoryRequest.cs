﻿namespace CateringService.Application.DataTransferObjects.Requests;

public sealed class UpdateMenuCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
}