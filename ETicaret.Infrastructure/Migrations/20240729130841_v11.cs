using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaret.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "418ba800-de08-40e0-b92a-1cfe98c3589a");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ea6ff782-be30-418b-abd9-fd53ce4165a0", null, "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea6ff782-be30-418b-abd9-fd53ce4165a0");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "418ba800-de08-40e0-b92a-1cfe98c3589a", null, "Admin", "ADMIN" });
        }
    }
}
