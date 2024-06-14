using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class FixIndexUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventDetail_LocationID",
                table: "EventDetail");

            migrationBuilder.CreateIndex(
                name: "IX_EventDetail_LocationID",
                table: "EventDetail",
                column: "LocationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventDetail_LocationID",
                table: "EventDetail");

            migrationBuilder.CreateIndex(
                name: "IX_EventDetail_LocationID",
                table: "EventDetail",
                column: "LocationID",
                unique: true);
        }
    }
}
