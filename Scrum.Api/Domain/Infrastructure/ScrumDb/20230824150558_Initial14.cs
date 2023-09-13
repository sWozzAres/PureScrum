using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstimatedDays",
                table: "ProductBacklogItems",
                newName: "EstimationPoints");

            migrationBuilder.AddColumn<int>(
                name: "EstimationPoints",
                table: "SprintBacklogItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimationPoints",
                table: "SprintBacklogItems");

            migrationBuilder.RenameColumn(
                name: "EstimationPoints",
                table: "ProductBacklogItems",
                newName: "EstimatedDays");
        }
    }
}
