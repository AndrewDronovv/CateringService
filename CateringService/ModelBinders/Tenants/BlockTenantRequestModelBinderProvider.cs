using CateringService.Application.DataTransferObjects.Requests;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CateringService.ModelBinders.Tenants;

public class BlockTenantRequestModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(BlockTenantRequest))
        {
            return new BinderTypeModelBinder(typeof(BlockTenantRequestModelBinder));
        }

        return null;
    }
}