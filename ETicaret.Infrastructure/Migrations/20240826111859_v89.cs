using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaret.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v89 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "574592b3-84ed-4dbc-a6b6-262fc7640700");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9ed2edbf-dd48-490a-9f13-cc3291d15bab", null, "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ed2edbf-dd48-490a-9f13-cc3291d15bab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "574592b3-84ed-4dbc-a6b6-262fc7640700", null, "Admin", "ADMIN" });
        }
    }
}
