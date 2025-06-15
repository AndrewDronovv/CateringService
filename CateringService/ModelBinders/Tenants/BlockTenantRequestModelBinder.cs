using CateringService.Application.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

namespace CateringService.ModelBinders.Tenants;

public class BlockTenantRequestModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (!bindingContext.ActionContext.RouteData.Values.TryGetValue("tenantId", out var tenantIdValue) ||
            tenantIdValue is null)
        {
            bindingContext.ModelState.AddModelError("tenantId", "TenantId отсутствует в маршруте.");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        if (!Ulid.TryParse(tenantIdValue.ToString(), out var tenantId))
        {
            bindingContext.ModelState.AddModelError("tenantId", "Неверный формат tenantId");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        bindingContext.HttpContext.Request.EnableBuffering();
        string body;
        using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            bindingContext.HttpContext.Request.Body.Position = 0;
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Тело запроса пустое.");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        try
        {
            var model = JsonSerializer.Deserialize<BlockTenantRequest>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (model == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Не удалось десериализовать тело запроса.");
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            model.Id = tenantId;

            bindingContext.Result = ModelBindingResult.Success(model);
        }
        catch (JsonException jsonEx)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, jsonEx, bindingContext.ModelMetadata);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}