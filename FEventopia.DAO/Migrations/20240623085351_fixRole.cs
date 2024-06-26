using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class fixRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<double>(
            //    name: "StallPrice",
            //    table: "EventDetail",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "EventAssignee",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StallPrice",
                table: "EventDetail");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "EventAssignee",
                type: "nvarchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");
        }
    }
}
