using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    BlockReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StreetAndBuilding = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Zip = table.Column<string>(type: "char(6)", maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Region = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "NULL"),
                    TenantId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BlockReason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<string>(type: "character varying(26)", nullable: false),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNumber = table.Column<int>(type: "integer", nullable: true),
                    Customer_CompanyId = table.Column<string>(type: "text", nullable: true),
                    AddressId = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategories_Users_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Ingredients = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Allergens = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PortionSize = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Slug = table.Column<string>(type: "text", nullable: true),
                    MenuCategoryId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_MenuCategories_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "01HY5K3D15E8BC6X9J9ZKBPNSM", "Company one" },
                    { "01HY5K3NCA4D8RYYWRZZ1RZD1X", "Company two" },
                    { "01HY5K3SH4XNFQ6MTFD1EZRAZB", "Company three" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "BlockReason", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { "01H5PY6RF4WKFCR9VCMY2QNFGP", "", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), true, "First tenant" },
                    { "01H5QJ6PVB8FYN4QXMR3T7JC9A", "", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), true, "Second tenant" },
                    { "01H5QJ7XQZKTYZ9QW8VRCMND5B", "", new DateTime(2025, 4, 22, 14, 15, 0, 0, DateTimeKind.Unspecified), true, "Third tenant" }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Comment", "Country", "CreatedAt", "Description", "Region", "StreetAndBuilding", "TenantId", "Zip" },
                values: new object[,]
                {
                    { "01H5QJ8KTMVRFZT58GQX902JD1", "New York", "Office address", "USA", new DateTime(2025, 4, 21, 8, 30, 0, 0, DateTimeKind.Unspecified), "Main headquarters", "NY", "123 Main St", "01H5PY6RF4WKFCR9VCMY2QNFGP", "100001" },
                    { "01H5QJ8RTMVRFZT58GQX902JD2", "Berlin", "Warehouse", "Germany", new DateTime(2025, 4, 22, 12, 15, 0, 0, DateTimeKind.Unspecified), "Storage facility", "Berlin", "45 Berliner Str.", "01H5QJ6PVB8FYN4QXMR3T7JC9A", "200002" },
                    { "01H5QJ9ZTMVRFZT58GQX902JD3", "Tokyo", "Retail store", "Japan", new DateTime(2025, 4, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), "Flagship location", "Kanto", "7-2 Shibuya", "01H5QJ7XQZKTYZ9QW8VRCMND5B", "300003" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BlockReason", "CompanyId", "CreatedAt", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "Phone", "Position", "TenantId", "UpdatedAt", "UserType" },
                values: new object[,]
                {
                    { "01HY5Q0RPNMXCA2W6JXDMVVZ7B", "", "01HY5K3D15E8BC6X9J9ZKBPNSM", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), "ikulikova@cateringservice.ru", "Irina", "Kulikova", "Alekseyevna", "hashed_password_here", "+7 (495) 123-45-67", "Supply Manager", "01H5PY6RF4WKFCR9VCMY2QNFGP", null, "Supplier" },
                    { "01HY5Q0WRK6VFYHT9BA3H8RK3V", "", "01HY5K3NCA4D8RYYWRZZ1RZD1X", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), "ivanov@cateringservice.ru", "Ivan", "Ivanov", "Ivanovich", "new_hashed_password", "+7 (495) 155-55-67", "Sales Manager", "01H5QJ6PVB8FYN4QXMR3T7JC9A", null, "Supplier" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BlockReason", "CreatedAt", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "Phone", "Role", "TenantId", "UpdatedAt", "UserType" },
                values: new object[] { "01HY5Q13CZD9FXT78GR1XWA2XB", null, new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), "dsorokin@brokeragepro.ru", "Dmitry", "Sorokin", "Petrovich", "hashed_secure_password", "+7 (495) 987-65-43", "Accountant", "01H5QJ7XQZKTYZ9QW8VRCMND5B", null, "Broker" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "BlockReason", "Customer_CompanyId", "CreatedAt", "Email", "FirstName", "LastName", "MiddleName", "PasswordHash", "Phone", "TaxNumber", "TenantId", "UpdatedAt", "UserType" },
                values: new object[] { "01HYZZZX7TS6AXK9R29X3PXJPX", null, null, "01HY5K3D15E8BC6X9J9ZKBPNSM", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), "osmirnova@cateringservice.ru", "Olga", "Smirnova", "Ivanovna", "hashed_customer_password", "+7 (495) 000-11-22", 123456789, "01H5PY6RF4WKFCR9VCMY2QNFGP", null, "Customer" });

            migrationBuilder.InsertData(
                table: "MenuCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "SupplierId" },
                values: new object[,]
                {
                    { "01H5QJ3DHBM8J6AW04FKPJP5VV", new DateTime(2025, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "Start your meal with our delightful appetizers.", "Appetizers", "01HY5Q0RPNMXCA2W6JXDMVVZ7B" },
                    { "01H5QJ3DJ22VXVG28Q0RYMNQEY", new DateTime(2025, 4, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), "Delicious main courses to satisfy your hunger.", "Main Courses", "01HY5Q0RPNMXCA2W6JXDMVVZ7B" },
                    { "01H5QJ3DR6R35WTKTPGFPJ89JC", new DateTime(2025, 4, 20, 14, 0, 0, 0, DateTimeKind.Unspecified), "End your meal with our sweet desserts.", "Desserts", "01HY5Q0WRK6VFYHT9BA3H8RK3V" }
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "Id", "Allergens", "CreatedAt", "Description", "ImageUrl", "Ingredients", "IsAvailable", "MenuCategoryId", "Name", "PortionSize", "Price", "Slug", "Weight" },
                values: new object[,]
                {
                    { "01GRQX9AYRHCA5Y5X3GPKPZ92P", "None", new DateTime(2025, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "Juicy grilled chicken with spices", "/images/GrilledChicken.webp", "Chicken, spices, olive oil", true, "01H5QJ3DHBM8J6AW04FKPJP5VV", "Grilled Chicken", "Large", 12.99m, null, 250.0 },
                    { "01GRQX9AYRHCA5Y5X3GPKPZ93Q", "None", new DateTime(2025, 4, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), "Fresh seasonal vegetables with olive oil", "/images/VegetableSalad.jpg", "Lettuce, tomatoes, cucumber, olive oil", true, "01H5QJ3DJ22VXVG28Q0RYMNQEY", "Vegetable Salad", "Medium", 8.50m, null, 150.0 },
                    { "01H5PY6RCAKEQ7VNK35P6XZ48Z", "Eggs, Milk", new DateTime(2025, 4, 20, 14, 0, 0, 0, DateTimeKind.Unspecified), "Rich and creamy chocolate cake", "/images/ChocolateCake.jpg", "Chocolate, flour, sugar, eggs, butter", false, "01H5QJ3DR6R35WTKTPGFPJ89JC", "Chocolate Cake", "Small", 5.99m, null, 300.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_StreetAndBuilding",
                table: "Addresses",
                columns: new[] { "City", "StreetAndBuilding" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TenantId",
                table: "Addresses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Zip",
                table: "Addresses",
                column: "Zip",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_Id",
                table: "Dishes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuCategoryId",
                table: "Dishes",
                column: "MenuCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_SupplierId",
                table: "MenuCategories",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
