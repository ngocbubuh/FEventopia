using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class fixeventdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StallPrice",
                table: "EventDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StallPrice",
                table: "EventDetail");
        }
    }
}
