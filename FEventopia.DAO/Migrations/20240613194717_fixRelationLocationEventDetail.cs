using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationLocationEventDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventDetail_Event_EventID",
                table: "EventDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_EventDetail_Event_EventID",
                table: "EventDetail",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventDetail_Event_EventID",
                table: "EventDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_EventDetail_Event_EventID",
                table: "EventDetail",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
