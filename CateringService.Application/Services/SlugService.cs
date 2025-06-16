using CateringService.Application.Abstractions;
using System.Text.RegularExpressions;

namespace CateringService.Application.Services;

public class SlugService : ISlugService
{
    public string GenerateSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        input = input.ToLower();
        input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
        input = Regex.Replace(input, @"\s+", " ").Trim();
        input = Regex.Replace(input, @"\s", "-");

        return input;
    }
}
