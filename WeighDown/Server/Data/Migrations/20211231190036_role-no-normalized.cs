using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeighDown.Server.Data.Migrations
{
    public partial class rolenonormalized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cdce201-4271-4409-9db8-051625c6c1ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c149ee9b-6031-42c5-8745-a421eba9fc6e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "093619d2-9ecc-45bb-aa7e-bc170db42005", "2981b18e-969a-4fb3-91ee-3139c4a0e481", "General", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0a04a6d-83b3-40d5-a66b-223051385afa", "e82fddb1-fcd9-4281-8fc1-56242add756e", "Admin", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "4cdce201-4271-4409-9db8-051625c6c1ec", "244e540d-3a66-4d61-8995-2911671a082b", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c149ee9b-6031-42c5-8745-a421eba9fc6e", "301832c0-270b-4214-b8e7-76970dfb445c", "Admin", "ADMIN" });
        }
    }
}
