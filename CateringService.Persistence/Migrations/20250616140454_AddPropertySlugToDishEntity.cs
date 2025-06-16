using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertySlugToDishEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Dishes",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: "01GRQX9AYRHCA5Y5X3GPKPZ92P",
                column: "Slug",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: "01GRQX9AYRHCA5Y5X3GPKPZ93Q",
                column: "Slug",
                value: null);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "DishId",
                keyValue: "01H5PY6RF4WKFCR9VCMY2QNFGP",
                column: "Slug",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Dishes");
        }
    }
}
