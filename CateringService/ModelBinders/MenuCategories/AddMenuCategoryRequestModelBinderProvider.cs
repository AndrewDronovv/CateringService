using CateringService.Application.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CateringService.ModelBinders.MenuCategories;

public class AddMenuCategoryRequestModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(AddMenuCategoryRequest))
        {
            return new BinderTypeModelBinder(typeof(AddMenuCategoryRequestModelBinder));
        }

        return null;
    }
}
