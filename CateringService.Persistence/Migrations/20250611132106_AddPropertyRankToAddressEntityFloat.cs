using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CateringService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyRankToAddressEntityFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rank",
                table: "Addresses",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ8KTMVRFZT58GQX902JD1",
                column: "Rank",
                value: null);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ8RTMVRFZT58GQX902JD2",
                column: "Rank",
                value: null);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ9ZTMVRFZT58GQX902JD3",
                column: "Rank",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rank",
                table: "Addresses",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ8KTMVRFZT58GQX902JD1",
                column: "Rank",
                value: null);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ8RTMVRFZT58GQX902JD2",
                column: "Rank",
                value: null);

            migrationBuilder.UpdateData(
                table: "Addresses",
                keyColumn: "AddressId",
                keyValue: "01H5QJ9ZTMVRFZT58GQX902JD3",
                column: "Rank",
                value: null);
        }
    }
}
