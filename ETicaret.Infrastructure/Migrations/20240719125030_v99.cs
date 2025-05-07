using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaret.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Campaign_CampaignId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e901f977-6207-4f21-9cf9-3cf5adad7299");

            migrationBuilder.AlterColumn<Guid>(
                name: "CampaignId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "82c5c94f-0b60-45a2-acca-0787e486cabb", null, "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Campaign_CampaignId",
                table: "Products",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Campaign_CampaignId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82c5c94f-0b60-45a2-acca-0787e486cabb");

            migrationBuilder.AlterColumn<Guid>(
                name: "CampaignId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e901f977-6207-4f21-9cf9-3cf5adad7299", null, "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Campaign_CampaignId",
                table: "Products",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
