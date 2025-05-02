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
                name: "Brokers",
                columns: table => new
                {
                    BrokerId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.BrokerId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustomerType = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TaxNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPersons",
                columns: table => new
                {
                    DeliveryPersonId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPersons", x => x.DeliveryPersonId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: ""),
                    TaxNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BrokerId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeliveryPersonId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_Deliveries_DeliveryPersons_DeliveryPersonId",
                        column: x => x.DeliveryPersonId,
                        principalTable: "DeliveryPersons",
                        principalColumn: "DeliveryPersonId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false),
                    BrokerId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "BrokerId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Invoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    MenuCategoryId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.MenuCategoryId);
                    table.ForeignKey(
                        name: "FK_MenuCategories_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Condition = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionId);
                    table.ForeignKey(
                        name: "FK_Promotions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    IncidentId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Resolution = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DeliveryId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_Incidents_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(26)", nullable: false),
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false),
                    DeliveryId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    DishId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
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
                    SupplierId = table.Column<string>(type: "character varying(26)", nullable: false),
                    MenuCategoryId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.DishId);
                    table.ForeignKey(
                        name: "FK_Dishes_MenuCategories_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "MenuCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dishes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderId = table.Column<string>(type: "character varying(26)", nullable: false),
                    DishId = table.Column<string>(type: "character varying(26)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Brokers",
                columns: new[] { "BrokerId", "ContactInfo", "Name" },
                values: new object[,]
                {
                    { "01H5QJ35QJ64MC1BTD5NRQ34R7", "info@gourmetcatering.com", "Gourmet Catering" },
                    { "01H5QJ36N1WHX5KDPQQGTVPVHC", "contact@healthykitchen.com", "Healthy Kitchen" },
                    { "01H5QJ379P7NZR1X03XW0GM7MA", "support@eventplanners.com", "Event Planners Co." }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CompanyName", "CustomerType", "FullName", "Phone", "TaxNumber" },
                values: new object[,]
                {
                    { "01H5QJ37V03WH5TXE2N1AW3JF9", null, "Individual", "John Doe", "+1-555-0123", null },
                    { "01H5QJ38KGWM2N56TFH99WQZ03", null, "Individual", "Jane Smith", "+1-555-0456", null },
                    { "01H5QJ391M8PVG6ZWPK4GTN0D8", "ACME Inc.", "Corporate", "Corporate Client", "+1-555-0789", "1234567890" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryPersons",
                columns: new[] { "DeliveryPersonId", "ContactInfo", "Name" },
                values: new object[,]
                {
                    { "01H5QJ3AFV0T3ZQBGP19HK2K5V", "alex.johnson@delivery.com", "Alex Johnson" },
                    { "01H5QJ3BBCEKJ7MYNVK302XRYF", "maria.gonzalez@delivery.com", "Maria Gonzalez" },
                    { "01H5QJ3BHR2FAYVZWNAD0XJJYE", "william.smith@delivery.com", "William Smith" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Address", "CompanyName", "ContactName", "Phone", "TaxNumber" },
                values: new object[,]
                {
                    { "01H5QJ6PTMVRFZT58GQX902JC4", "123 Market Street, City A", "Fresh Produce Supplier", "John Doe", "+1234567890", "123456789" },
                    { "01H5QJ6PVB8FYN4QXMR3T7JC9A", "456 Business Blvd, City B", "Global Catering Supplies", "Jane Smith", "+0987654321", "987654321" },
                    { "01H5QJ6PX4FTQY8KZVW9JMBT96", "789 Green Lane, City C", "Organic Goods Co.", "Alice Johnson", "+1122334455", "112233445" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "CreatedAt", "IsActive", "Name" },
                values: new object[,]
                {
                    { "01H5PY6RF4WKFCR9VCMY2QNFGP", new DateTime(2025, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), true, "First tenant" },
                    { "01H5QJ6PVB8FYN4QXMR3T7JC9A", new DateTime(2025, 4, 21, 12, 30, 0, 0, DateTimeKind.Unspecified), true, "Second tenant" },
                    { "01H5QJ7XQZKTYZ9QW8VRCMND5B", new DateTime(2025, 4, 22, 14, 15, 0, 0, DateTimeKind.Unspecified), true, "Third tenant" }
                });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "DeliveryId", "DeliveryPersonId", "Status" },
                values: new object[,]
                {
                    { "01H5QJ399WTKN11Z9FMB02WT62", "01H5QJ3AFV0T3ZQBGP19HK2K5V", "In Progress" },
                    { "01H5QJ39VRZ2AN3YC94PM5FMPA", "01H5QJ3BBCEKJ7MYNVK302XRYF", "Completed" },
                    { "01H5QJ3A8D7V2GPF2K4K3WH5C4", "01H5QJ3BHR2FAYVZWNAD0XJJYE", "Delayed" }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "Amount", "BrokerId", "DateIssued", "Status", "SupplierId" },
                values: new object[,]
                {
                    { "01H5QJ3CZ4FBZAMT62XXYY24FZ", 500.00m, "01H5QJ35QJ64MC1BTD5NRQ34R7", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid", "01H5QJ6PTMVRFZT58GQX902JC4" },
                    { "01H5QJ3D5T7JV9B1VQF6BRFV4P", 1500.50m, "01H5QJ36N1WHX5KDPQQGTVPVHC", new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unpaid", "01H5QJ6PVB8FYN4QXMR3T7JC9A" },
                    { "01H5QJ3DF6RQG96Q3VK7JBY58N", 800.75m, "01H5QJ379P7NZR1X03XW0GM7MA", new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "01H5QJ6PX4FTQY8KZVW9JMBT96" }
                });

            migrationBuilder.InsertData(
                table: "MenuCategories",
                columns: new[] { "MenuCategoryId", "CreatedAt", "Description", "Name", "SupplierId" },
                values: new object[,]
                {
                    { "01H5QJ3DHBM8J6AW04FKPJP5VV", new DateTime(2025, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "Start your meal with our delightful appetizers.", "Appetizers", "01H5QJ6PTMVRFZT58GQX902JC4" },
                    { "01H5QJ3DJ22VXVG28Q0RYMNQEY", new DateTime(2025, 4, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), "Delicious main courses to satisfy your hunger.", "Main Courses", "01H5QJ6PVB8FYN4QXMR3T7JC9A" },
                    { "01H5QJ3DR6R35WTKTPGFPJ89JC", new DateTime(2025, 4, 20, 14, 0, 0, 0, DateTimeKind.Unspecified), "End your meal with our sweet desserts.", "Desserts", "01H5QJ6PX4FTQY8KZVW9JMBT96" }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "PromotionId", "Condition", "DiscountValue", "EndDate", "StartDate", "SupplierId", "Type" },
                values: new object[,]
                {
                    { "01H5QJ6P88F6YXPNKJX42VFYB5", "Minimum order $100", 15.00m, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ6PTMVRFZT58GQX902JC4", "Percentage" },
                    { "01H5QJ6PCZJ70AW3MMFGXK5TBQ", "For first-time customers", 20.00m, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ6PVB8FYN4QXMR3T7JC9A", "Fixed Amount" },
                    { "01H5QJ6PFAWWNG1T52BZ20RQFX", "For orders over $50", 0.00m, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ6PX4FTQY8KZVW9JMBT96", "Free Delivery" }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "BrokerId", "Details", "GeneratedDate", "Type" },
                values: new object[,]
                {
                    { "01H5QJ6PJXP3KN3ZMCXGTFY8P9", "01H5QJ35QJ64MC1BTD5NRQ34R7", "Detailed performance report for Q1.", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Performance" },
                    { "01H5QJ6PMZ48BVTCJMK30RW9J6", "01H5QJ36N1WHX5KDPQQGTVPVHC", "Compliance report for catering regulations.", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compliance" },
                    { "01H5QJ6PRJAXFV54N82M3TQXJY", "01H5QJ379P7NZR1X03XW0GM7MA", "Comprehensive financial analysis for last quarter.", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Financial" }
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "DishId", "Allergens", "CreatedAt", "Description", "ImageUrl", "Ingredients", "IsAvailable", "MenuCategoryId", "Name", "PortionSize", "Price", "SupplierId", "Weight" },
                values: new object[,]
                {
                    { "01GRQX9AYRHCA5Y5X3GPKPZ92P", "None", new DateTime(2025, 4, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), "Juicy grilled chicken with spices", "/images/GrilledChicken.webp", "Chicken, spices, olive oil", true, "01H5QJ3DHBM8J6AW04FKPJP5VV", "Grilled Chicken", "Large", 12.99m, "01H5QJ6PTMVRFZT58GQX902JC4", 250.0 },
                    { "01GRQX9AYRHCA5Y5X3GPKPZ93Q", "None", new DateTime(2025, 4, 20, 12, 0, 0, 0, DateTimeKind.Unspecified), "Fresh seasonal vegetables with olive oil", "/images/VegetableSalad.jpg", "Lettuce, tomatoes, cucumber, olive oil", true, "01H5QJ3DJ22VXVG28Q0RYMNQEY", "Vegetable Salad", "Medium", 8.50m, "01H5QJ6PVB8FYN4QXMR3T7JC9A", 150.0 },
                    { "01H5PY6RF4WKFCR9VCMY2QNFGP", "Eggs, Milk", new DateTime(2025, 4, 20, 14, 0, 0, 0, DateTimeKind.Unspecified), "Rich and creamy chocolate cake", "/images/ChocolateCake.jpg", "Chocolate, flour, sugar, eggs, butter", false, "01H5QJ3DR6R35WTKTPGFPJ89JC", "Chocolate Cake", "Small", 5.99m, "01H5QJ6PX4FTQY8KZVW9JMBT96", 300.0 }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentId", "Date", "DeliveryId", "Description", "Resolution" },
                values: new object[,]
                {
                    { "01H5QJ3BTSX3JJ3F6DTQVFX86P", new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ399WTKN11Z9FMB02WT62", "Late delivery due to traffic jam", "Customer notified and accepted delay" },
                    { "01H5QJ3CB21J8GEPKGXZ80WRQ9", new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ39VRZ2AN3YC94PM5FMPA", "Damaged package during delivery", "Replacement item sent to customer" },
                    { "01H5QJ3CC0PF6XRTA21DW3QPEK", new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ3A8D7V2GPF2K4K3WH5C4", "Wrong address provided by customer", "Correct address obtained and delivery rescheduled" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DeliveryDate", "DeliveryId", "OrderDate", "Status", "SupplierId", "TotalPrice" },
                values: new object[,]
                {
                    { "01H5QJ3DZP8N3A1EQNHQZK7GTT", "01H5QJ37V03WH5TXE2N1AW3JF9", new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ399WTKN11Z9FMB02WT62", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "01H5QJ6PTMVRFZT58GQX902JC4", 250.00m },
                    { "01H5QJ3E1TZPGJ82MMZ20WX44Z", "01H5QJ38KGWM2N56TFH99WQZ03", new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ39VRZ2AN3YC94PM5FMPA", new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", "01H5QJ6PVB8FYN4QXMR3T7JC9A", 150.75m },
                    { "01H5QJ3E3P7D4X8KVT4X30PKKQ", "01H5QJ391M8PVG6ZWPK4GTN0D8", new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "01H5QJ3A8D7V2GPF2K4K3WH5C4", new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancelled", "01H5QJ6PX4FTQY8KZVW9JMBT96", 300.50m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "DishId", "OrderId", "Price", "Quantity" },
                values: new object[,]
                {
                    { "01H5QJ3E5929D8TFHK4M4PK0YE", "01GRQX9AYRHCA5Y5X3GPKPZ92P", "01H5QJ3DZP8N3A1EQNHQZK7GTT", 25.00m, 2 },
                    { "01H5QJ3E72PFV0T3XN92K4W59V", "01GRQX9AYRHCA5Y5X3GPKPZ93Q", "01H5QJ3E1TZPGJ82MMZ20WX44Z", 15.50m, 1 },
                    { "01H5QJ6P1YKRV9FX54Z0W3PJAY", "01H5PY6RF4WKFCR9VCMY2QNFGP", "01H5QJ3E3P7D4X8KVT4X30PKKQ", 45.75m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryPersonId",
                table: "Deliveries",
                column: "DeliveryPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_DishId",
                table: "Dishes",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuCategoryId",
                table: "Dishes",
                column: "MenuCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_SupplierId",
                table: "Dishes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_DeliveryId",
                table: "Incidents",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BrokerId",
                table: "Invoices",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SupplierId",
                table: "Invoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_SupplierId",
                table: "MenuCategories",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DishId",
                table: "OrderItems",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierId",
                table: "Orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_SupplierId",
                table: "Promotions",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_BrokerId",
                table: "Reports",
                column: "BrokerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "DeliveryPersons");
        }
    }
}
