using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                table: "Products",
                newName: "UQ_Products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIncrements_Name",
                table: "ProductIncrements",
                newName: "UQ_ProductIncrements_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UQ_Products_Name",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.RenameIndex(
                name: "UQ_ProductIncrements_Name",
                table: "ProductIncrements",
                newName: "IX_ProductIncrements_Name");
        }
    }
}
