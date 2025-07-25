namespace CateringService;

public static class ApiEndPoints
{
    private const string ApiBase = "api";

    public static class MenuCategories
    {
        private const string Base = $"{ApiBase}/suppliers/{{supplierId}}/categories";

        public const string GetAll = $"{Base}";
        public const string Get = $"{Base}/{{menuCategoryId}}";
        public const string Create = $"{Base}";
        public const string Update = $"{Base}/{{menuCategoryId}}";
        public const string Delete = $"{Base}/{{menuCategoryId}}";
    }

    public static class Tenants
    {
        private const string Base = $"{ApiBase}/tenants";

        public const string GetAll = $"{Base}";
        public const string Get = $"{Base}/{{tenantId}}";
        public const string Create = $"{Base}";
        public const string Delete = $"{Base}/{{tenantId}}";
        public const string Update = $"{Base}/{{tenantId}}";
        public const string Block = $"{Base}/{{tenantId}}/block";
        public const string Unblock = $"{Base}/{{tenantId}}/unblock";
    }

    public static class Addresses
    {
        private const string Base = $"{ApiBase}/addresses";

        public const string Create = $"{Base}";
        public const string Get = $"{Base}/{{addressId}}";
        public const string Update = $"{Base}/{{addressId}}";
    }
}