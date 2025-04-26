namespace CateringService;

public static class ApiEndPoints
{
    private const string ApiBase = "api";

    public static class Dishes
    {
        private const string Base = $"{ApiBase}/dishes";

        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class MenuCategories
    {
        private const string Base = $"{ApiBase}/suppliers/{{supplierId}}/categories";

        public const string GetAll = $"{Base}";
        public const string Get = $"{Base}/{{categoryId}}";
        public const string Create = $"{Base}";
        public const string Delete = $"{Base}/{{categoryId}}";
    }
}