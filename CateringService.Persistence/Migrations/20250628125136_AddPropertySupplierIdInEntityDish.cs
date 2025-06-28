using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertySupplierIdInEntityDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupplierId",
                table: "Dishes",
                type: "character varying(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: "01GRQX9AYRHCA5Y5X3GPKPZ92P",
                column: "SupplierId",
                value: "01HY5Q0RPNMXCA2W6JXDMVVZ7B");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: "01GRQX9AYRHCA5Y5X3GPKPZ93Q",
                column: "SupplierId",
                value: "01HY5Q0RPNMXCA2W6JXDMVVZ7B");

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "Id",
                keyValue: "01H5PY6RCAKEQ7VNK35P6XZ48Z",
                column: "SupplierId",
                value: "01HY5Q0RPNMXCA2W6JXDMVVZ7B");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_SupplierId",
                table: "Dishes",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Users_SupplierId",
                table: "Dishes",
                column: "SupplierId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Users_SupplierId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_SupplierId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Dishes");
        }
    }
}
