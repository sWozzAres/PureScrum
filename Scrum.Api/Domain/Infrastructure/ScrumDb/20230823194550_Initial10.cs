using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBacklogItemProductBacklogItem");

            migrationBuilder.CreateTable(
                name: "ProductBacklogItemDependencies",
                columns: table => new
                {
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBacklogItemDependencies", x => new { x.ParentId, x.ChildId });
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ChildId",
                        column: x => x.ChildId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemDependencies_ProductBacklogItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItemDependencies_ChildId",
                table: "ProductBacklogItemDependencies",
                column: "ChildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBacklogItemDependencies");

            migrationBuilder.CreateTable(
                name: "ProductBacklogItemProductBacklogItem",
                columns: table => new
                {
                    ChildrenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBacklogItemProductBacklogItem", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemProductBacklogItem_ProductBacklogItems_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBacklogItemProductBacklogItem_ProductBacklogItems_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "ProductBacklogItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBacklogItemProductBacklogItem_ParentsId",
                table: "ProductBacklogItemProductBacklogItem",
                column: "ParentsId");
        }
    }
}
