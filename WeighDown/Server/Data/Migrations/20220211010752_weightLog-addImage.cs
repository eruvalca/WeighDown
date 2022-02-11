using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeighDown.Server.Data.Migrations
{
    public partial class weightLogaddImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09cfb017-7b9c-478a-aded-61bcb260801d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "311eae48-06ef-4465-aa0a-487081749e2a");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "WeightLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RecognizedWeightMeasurement",
                table: "WeightLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "92c5e2fa-b297-41d5-a525-ae5acd73a712", "570b25e9-c069-4225-bb17-f1010391c4af", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b3756e0-e0c0-49d0-9e9d-bf08f5bf201e", "d62c5802-5499-4aeb-b67e-a51fa9ce6322", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92c5e2fa-b297-41d5-a525-ae5acd73a712");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b3756e0-e0c0-49d0-9e9d-bf08f5bf201e");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "WeightLogs");

            migrationBuilder.DropColumn(
                name: "RecognizedWeightMeasurement",
                table: "WeightLogs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "09cfb017-7b9c-478a-aded-61bcb260801d", "3d13bd76-cbdb-4540-ae48-7557397449ee", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "311eae48-06ef-4465-aa0a-487081749e2a", "4d8b47ce-6ddd-4a75-a295-5c5650ec0bba", "General", "GENERAL" });
        }
    }
}
