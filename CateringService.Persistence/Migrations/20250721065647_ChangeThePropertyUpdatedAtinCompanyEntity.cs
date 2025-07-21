using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeThePropertyUpdatedAtinCompanyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "NULL");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "01HY5K3D15E8BC6X9J9ZKBPNSM",
                column: "UpdatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "01HY5K3NCA4D8RYYWRZZ1RZD1X",
                column: "UpdatedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "01HY5K3SH4XNFQ6MTFD1EZRAZB",
                column: "UpdatedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "NULL",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
