using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobWellTest2.Migrations
{
    /// <inheritdoc />
    public partial class NewInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "730f8efd-6eeb-4af8-bb5e-3005abf0d920");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95fd446d-dbc0-4e71-acc2-723b4ade4028");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ddb9ce1-90b8-48d8-baf6-a318c7496910", null, "Admin", "ADMIN" },
                    { "27925f23-4ce8-4a55-a92b-c759a31bf066", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ddb9ce1-90b8-48d8-baf6-a318c7496910");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27925f23-4ce8-4a55-a92b-c759a31bf066");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "730f8efd-6eeb-4af8-bb5e-3005abf0d920", null, "User", "USER" },
                    { "95fd446d-dbc0-4e71-acc2-723b4ade4028", null, "Admin", "ADMIN" }
                });
        }
    }
}
