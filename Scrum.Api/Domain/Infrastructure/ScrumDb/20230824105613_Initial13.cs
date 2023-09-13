using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "SprintBacklogItems");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SprintBacklogItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SprintBacklogItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "SprintBacklogItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
