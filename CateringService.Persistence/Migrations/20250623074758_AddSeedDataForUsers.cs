using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01HY5Q0RPNMXCA2W6JXDMVVZ7B");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01HY5Q0WRK6VFYHT9BA3H8RK3V");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "01HY5Q13CZD9FXT78GR1XWA2XB");
        }
    }
}
