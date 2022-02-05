using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeighDown.Server.Data.Migrations
{
    public partial class prizeFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "061af4ee-cf5a-4c23-ae54-c583d0ff0527");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a3b4249-735a-4e13-856f-4a3cde70dea9");

            migrationBuilder.AddColumn<decimal>(
                name: "FirstPlacePrizeAmount",
                table: "Competitions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SecondPlacePrizeAmount",
                table: "Competitions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThirdPlacePrizeAmount",
                table: "Competitions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "09cfb017-7b9c-478a-aded-61bcb260801d", "3d13bd76-cbdb-4540-ae48-7557397449ee", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "311eae48-06ef-4465-aa0a-487081749e2a", "4d8b47ce-6ddd-4a75-a295-5c5650ec0bba", "General", "GENERAL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09cfb017-7b9c-478a-aded-61bcb260801d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "311eae48-06ef-4465-aa0a-487081749e2a");

            migrationBuilder.DropColumn(
                name: "FirstPlacePrizeAmount",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "SecondPlacePrizeAmount",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "ThirdPlacePrizeAmount",
                table: "Competitions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "061af4ee-cf5a-4c23-ae54-c583d0ff0527", "07334965-9892-43c1-95f5-625e10bea345", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a3b4249-735a-4e13-856f-4a3cde70dea9", "8a3e2bd6-99a2-4d78-8d30-a4e29396cb3b", "Admin", "ADMIN" });
        }
    }
}
