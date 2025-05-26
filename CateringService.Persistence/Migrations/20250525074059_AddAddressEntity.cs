using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                    TenantId = table.Column<string>(type: "character varying(26)", nullable: false),
                    Country = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    StreetAndBuilding = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Zip = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    City = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Region = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "City", "Comment", "Country", "CreatedAt", "Description", "Region", "StreetAndBuilding", "TenantId", "Zip" },
                values: new object[,]
                {
                    { "01H5QJ8KTMVRFZT58GQX902JD1", "New York", "Office address", "USA", new DateTime(2025, 4, 21, 8, 30, 0, 0, DateTimeKind.Unspecified), "Main headquarters", "NY", "123 Main St", "01H5PY6RF4WKFCR9VCMY2QNFGP", "100001" },
                    { "01H5QJ8RTMVRFZT58GQX902JD2", "Berlin", "Warehouse", "Germany", new DateTime(2025, 4, 22, 12, 15, 0, 0, DateTimeKind.Unspecified), "Storage facility", "Berlin", "45 Berliner Str.", "01H5QJ6PVB8FYN4QXMR3T7JC9A", "200002" },
                    { "01H5QJ9ZTMVRFZT58GQX902JD3", "Tokyo", "Retail store", "Japan", new DateTime(2025, 4, 23, 9, 0, 0, 0, DateTimeKind.Unspecified), "Flagship location", "Kanto", "7-2 Shibuya", "01H5QJ7XQZKTYZ9QW8VRCMND5B", "300003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TenantId",
                table: "Addresses",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Tenants",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
