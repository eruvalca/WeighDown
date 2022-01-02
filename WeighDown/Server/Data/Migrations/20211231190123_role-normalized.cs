using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeighDown.Server.Data.Migrations
{
    public partial class rolenormalized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "093619d2-9ecc-45bb-aa7e-bc170db42005");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0a04a6d-83b3-40d5-a66b-223051385afa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "13a59e49-861f-4370-8778-cd2d33cd0f90", "d5bf2381-1eab-433c-9605-6817c0915e70", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e760f5ac-6179-402d-a1f9-d9daeef9905f", "d41e41cd-7c8c-4da3-b9ab-a29f36312015", "General", "GENERAL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13a59e49-861f-4370-8778-cd2d33cd0f90");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e760f5ac-6179-402d-a1f9-d9daeef9905f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "093619d2-9ecc-45bb-aa7e-bc170db42005", "2981b18e-969a-4fb3-91ee-3139c4a0e481", "General", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0a04a6d-83b3-40d5-a66b-223051385afa", "e82fddb1-fcd9-4281-8fc1-56242add756e", "Admin", null });
        }
    }
}
