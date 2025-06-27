using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableToUpdatedAtCompanyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "NULL",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "NULL",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValueSql: "NULL");
        }
    }
}
