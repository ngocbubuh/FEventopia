using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSponsorMngKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SponsorManagement",
                table: "SponsorManagement");

            migrationBuilder.DropIndex(
                name: "IX_SponsorManagement_EventId",
                table: "SponsorManagement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SponsorManagement",
                table: "SponsorManagement",
                columns: new[] { "EventId", "SponsorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SponsorManagement",
                table: "SponsorManagement");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SponsorManagement",
                table: "SponsorManagement",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorManagement_EventId",
                table: "SponsorManagement",
                column: "EventId");
        }
    }
}
