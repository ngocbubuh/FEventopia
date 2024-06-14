using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddActualAmountSponsor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "SponsorManagement",
                newName: "PledgeAmount");

            migrationBuilder.AddColumn<double>(
                name: "ActualAmount",
                table: "SponsorManagement",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualAmount",
                table: "SponsorManagement");

            migrationBuilder.RenameColumn(
                name: "PledgeAmount",
                table: "SponsorManagement",
                newName: "Amount");
        }
    }
}
