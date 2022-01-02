using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeighDown.Server.Data.Migrations
{
    public partial class competitionmoneycol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "493aa5f8-4768-4071-bbd0-4f17d0ff33b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "649b9212-1e71-413a-8b55-cf409e9528a5");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlayInAmount",
                table: "Competitions",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "061af4ee-cf5a-4c23-ae54-c583d0ff0527", "07334965-9892-43c1-95f5-625e10bea345", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a3b4249-735a-4e13-856f-4a3cde70dea9", "8a3e2bd6-99a2-4d78-8d30-a4e29396cb3b", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "061af4ee-cf5a-4c23-ae54-c583d0ff0527");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a3b4249-735a-4e13-856f-4a3cde70dea9");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlayInAmount",
                table: "Competitions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "493aa5f8-4768-4071-bbd0-4f17d0ff33b7", "199f7038-d7bf-4625-90af-cae33467ff9d", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "649b9212-1e71-413a-8b55-cf409e9528a5", "c8d56c21-fd54-43c3-bd89-5735d416874c", "General", "GENERAL" });
        }
    }
}
