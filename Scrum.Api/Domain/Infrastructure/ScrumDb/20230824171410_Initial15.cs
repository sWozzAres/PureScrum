using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_ProductIncrements_ProductIncrementId",
                table: "ProductBacklogItems");

            migrationBuilder.DropTable(
                name: "ProductIncrements");

            migrationBuilder.RenameColumn(
                name: "ProductIncrementId",
                table: "ProductBacklogItems",
                newName: "SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBacklogItems_ProductIncrementId",
                table: "ProductBacklogItems",
                newName: "IX_ProductBacklogItems_SprintId");

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintGoal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ExpectedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_Sprints_Name",
                table: "Sprints",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBacklogItems_Sprints_SprintId",
                table: "ProductBacklogItems");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "ProductBacklogItems",
                newName: "ProductIncrementId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBacklogItems_SprintId",
                table: "ProductBacklogItems",
                newName: "IX_ProductBacklogItems_ProductIncrementId");

            migrationBuilder.CreateTable(
                name: "ProductIncrements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Owner = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintGoal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIncrements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_ProductIncrements_Name",
                table: "ProductIncrements",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBacklogItems_ProductIncrements_ProductIncrementId",
                table: "ProductBacklogItems",
                column: "ProductIncrementId",
                principalTable: "ProductIncrements",
                principalColumn: "Id");
        }
    }
}
