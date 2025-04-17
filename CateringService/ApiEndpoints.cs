using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CateringService;

public static class ApiEndPoints
{
    private const string ApiBase = "api";

    public static class Dishes
    {
        private const string Base = $"{ApiBase}/dishes";

        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id:int}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id:int}}";
        public const string Delete = $"{Base}/{{id:int}}";
    }
}