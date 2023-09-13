using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    /// <inheritdoc />
    public partial class Initial35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintBacklogItems_SprintBacklogItems_SprintBacklogItemId",
                table: "SprintBacklogItems");

            migrationBuilder.DropIndex(
                name: "IX_SprintBacklogItems_SprintBacklogItemId",
                table: "SprintBacklogItems");

            migrationBuilder.DropColumn(
                name: "SprintBacklogItemId",
                table: "SprintBacklogItems")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SprintBacklogItemsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SprintBacklogItemDependencies",
                columns: table => new
                {
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintBacklogItemDependencies", x => new { x.ParentId, x.ChildId });
                    table.ForeignKey(
                        name: "FK_SprintBacklogItemDependencies_SprintBacklogItems_ChildId",
                        column: x => x.ChildId,
                        principalTable: "SprintBacklogItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SprintBacklogItemDependencies_SprintBacklogItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SprintBacklogItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SprintBacklogItemDependencies_ChildId",
                table: "SprintBacklogItemDependencies",
                column: "ChildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SprintBacklogItemDependencies");

            migrationBuilder.AddColumn<Guid>(
                name: "SprintBacklogItemId",
                table: "SprintBacklogItems",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SprintBacklogItemsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_SprintBacklogItems_SprintBacklogItemId",
                table: "SprintBacklogItems",
                column: "SprintBacklogItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintBacklogItems_SprintBacklogItems_SprintBacklogItemId",
                table: "SprintBacklogItems",
                column: "SprintBacklogItemId",
                principalTable: "SprintBacklogItems",
                principalColumn: "Id");
        }
    }
}
