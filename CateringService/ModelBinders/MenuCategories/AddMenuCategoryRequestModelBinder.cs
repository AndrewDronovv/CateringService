using CateringService.Application.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

namespace CateringService.ModelBinders.MenuCategories;

public class AddMenuCategoryRequestModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (!bindingContext.ActionContext.RouteData.Values.TryGetValue("supplierId", out var supplierIdValue) ||
            supplierIdValue is null)
        {
            bindingContext.ModelState.AddModelError("supplierId", "SupplierId отсутствует в маршруте.");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        if (!Ulid.TryParse(supplierIdValue.ToString(), out var supplierId))
        {
            bindingContext.ModelState.AddModelError("supplierId", "Неверный формат supplierId");
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
            var model = JsonSerializer.Deserialize<AddMenuCategoryRequest>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (model == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Не удалось десериализовать тело запроса.");
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            model.SupplierId = supplierId;

            bindingContext.Result = ModelBindingResult.Success(model);
        }
        catch (JsonException jsonEx)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, jsonEx, bindingContext.ModelMetadata);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
