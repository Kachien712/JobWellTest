using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobWellTest2.Migrations
{
    /// <inheritdoc />
    public partial class CitizenID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c6a4030-f73b-43cf-92c8-92491d46433f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51fd6123-791e-4ccb-b9d5-02e5d2dd5093");

            migrationBuilder.AddColumn<string>(
                name: "CitizenIdentityNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "730f8efd-6eeb-4af8-bb5e-3005abf0d920", null, "User", "USER" },
                    { "95fd446d-dbc0-4e71-acc2-723b4ade4028", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "730f8efd-6eeb-4af8-bb5e-3005abf0d920");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95fd446d-dbc0-4e71-acc2-723b4ade4028");

            migrationBuilder.DropColumn(
                name: "CitizenIdentityNumber",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0c6a4030-f73b-43cf-92c8-92491d46433f", null, "User", "USER" },
                    { "51fd6123-791e-4ccb-b9d5-02e5d2dd5093", null, "Admin", "ADMIN" }
                });
        }
    }
}
