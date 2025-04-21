using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    BrokerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPersons",
                columns: table => new
                {
                    DeliveryPersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    SupplierId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Описание поставщика"),
                    Logo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Ссылка на логотип поставщика"),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WorkingHours = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BrokerId = table.Column<int>(type: "integer", nullable: false)
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
                    DeliveryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeliveryPersonId = table.Column<int>(type: "integer", nullable: false)
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
                    InvoiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    BrokerId = table.Column<int>(type: "integer", nullable: false)
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
                name: "MenuSections",
                columns: table => new
                {
                    MenuSectionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuSections", x => x.MenuSectionId);
                    table.ForeignKey(
                        name: "FK_MenuSections_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Condition = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false)
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
                    IncidentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Resolution = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DeliveryId = table.Column<int>(type: "integer", nullable: false)
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
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    DeliveryId = table.Column<int>(type: "integer", nullable: false)
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
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Ingredients = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    Image = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AvailabilityStatus = table.Column<bool>(type: "boolean", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    MenuSectionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.DishId);
                    table.ForeignKey(
                        name: "FK_Dishes_MenuSections_MenuSectionId",
                        column: x => x.MenuSectionId,
                        principalTable: "MenuSections",
                        principalColumn: "MenuSectionId",
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
                    OrderItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
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
                    { 1, "info@gourmetcatering.com", "Gourmet Catering" },
                    { 2, "contact@healthykitchen.com", "Healthy Kitchen" },
                    { 3, "support@eventplanners.com", "Event Planners Co." }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "ContactInfo", "Name", "PaymentType" },
                values: new object[,]
                {
                    { 1, "john.doe@example.com", "John Doe", "CreditCard" },
                    { 2, "jane.smith@domain.com", "Jane Smith", "PayPal" },
                    { 3, "contact@corporate.com", "Corporate Client", "Cash" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryPersons",
                columns: new[] { "DeliveryPersonId", "ContactInfo", "Name" },
                values: new object[,]
                {
                    { 1, "alex.johnson@delivery.com", "Alex Johnson" },
                    { 2, "maria.gonzalez@delivery.com", "Maria Gonzalez" },
                    { 3, "william.smith@delivery.com", "William Smith" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Description", "Email", "IsActive", "Logo", "Name", "Phone", "WorkingHours" },
                values: new object[,]
                {
                    { 1, "Поставщик свежих продуктов для ресторанов", "contact@freshproduce.com", true, "https://example.com/logo1.png", "Fresh Produce Supplier", "+1234567890", 8 },
                    { 2, "Глобальный поставщик кейтерингового оборудования", "info@globalcatering.com", true, "https://example.com/logo2.png", "Global Catering Supplies", "+0987654321", 10 }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Description", "Email", "Logo", "Name", "Phone", "WorkingHours" },
                values: new object[] { 3, "Поставщик органических продуктов питания", "sales@organicgoods.com", "https://example.com/logo3.png", "Organic Goods Co.", "+1122334455", 6 });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "DeliveryId", "DeliveryPersonId", "Status" },
                values: new object[,]
                {
                    { 1, 1, "In Progress" },
                    { 2, 2, "Completed" },
                    { 3, 3, "Delayed" }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "InvoiceId", "Amount", "BrokerId", "DateIssued", "Status", "SupplierId" },
                values: new object[,]
                {
                    { 1, 500.00m, 1, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid", 1 },
                    { 2, 1500.50m, 2, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unpaid", 2 },
                    { 3, 800.75m, 3, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", 3 }
                });

            migrationBuilder.InsertData(
                table: "MenuSections",
                columns: new[] { "MenuSectionId", "Name", "SupplierId" },
                values: new object[,]
                {
                    { 1, "Appetizers", 1 },
                    { 2, "Main Courses", 2 },
                    { 3, "Desserts", 3 }
                });

            migrationBuilder.InsertData(
                table: "Promotions",
                columns: new[] { "PromotionId", "Condition", "DiscountValue", "EndDate", "StartDate", "SupplierId", "Type" },
                values: new object[,]
                {
                    { 1, "Minimum order $100", 15.00m, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Percentage" },
                    { 2, "For first-time customers", 20.00m, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Fixed Amount" },
                    { 3, "For orders over $50", 0.00m, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Free Delivery" }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "BrokerId", "Details", "GeneratedDate", "Type" },
                values: new object[,]
                {
                    { 1, 1, "Detailed performance report for Q1.", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Performance" },
                    { 2, 2, "Compliance report for catering regulations.", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compliance" },
                    { 3, 3, "Comprehensive financial analysis for last quarter.", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Financial" }
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "DishId", "AvailabilityStatus", "Description", "Image", "Ingredients", "MenuSectionId", "Name", "Price", "SupplierId", "Weight" },
                values: new object[,]
                {
                    { "01GRQX9AYRHCA5Y5X3GPKPZ92P", true, "Juicy grilled chicken with spices", "grilled_chicken.jpg", "Chicken, spices, olive oil", 1, "Grilled Chicken", 12.99m, 1, 250.0 },
                    { "01GRQX9AYRHCA5Y5X3GPKPZ93Q", true, "Fresh seasonal vegetables with olive oil", "veggie_salad.jpg", "Lettuce, tomatoes, cucumber, olive oil", 2, "Vegetable Salad", 8.50m, 2, 150.0 },
                    { "01H5PY6RF4WKFCR9VCMY2QNFGP", false, "Rich and creamy chocolate cake", "chocolate_cake.jpg", "Chocolate, flour, sugar, eggs, butter", 3, "Chocolate Cake", 5.99m, 3, 300.0 }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentId", "Date", "DeliveryId", "Description", "Resolution" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Late delivery due to traffic jam", "Customer notified and accepted delay" },
                    { 2, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Damaged package during delivery", "Replacement item sent to customer" },
                    { 3, new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Wrong address provided by customer", "Correct address obtained and delivery rescheduled" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DeliveryDate", "DeliveryId", "OrderDate", "Status", "SupplierId", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Completed", 1, 250.00m },
                    { 2, 2, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", 2, 150.75m },
                    { 3, 3, new DateTime(2025, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancelled", 3, 300.50m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 25.00m, 2 },
                    { 2, 2, 15.50m, 1 },
                    { 3, 3, 45.75m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryPersonId",
                table: "Deliveries",
                column: "DeliveryPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_MenuSectionId",
                table: "Dishes",
                column: "MenuSectionId");

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
                name: "IX_MenuSections_SupplierId",
                table: "MenuSections",
                column: "SupplierId");

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
                name: "Dishes");

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
                name: "MenuSections");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Brokers");

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
