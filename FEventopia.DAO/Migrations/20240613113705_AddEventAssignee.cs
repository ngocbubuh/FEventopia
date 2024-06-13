using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddEventAssignee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Account_OperatorID",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_OperatorID",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "OperatorID",
                table: "Event");

            migrationBuilder.AlterColumn<double>(
                name: "TicketPrice",
                table: "EventDetail",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "StallForSaleInventory",
                table: "EventDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventAssignee",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteFlag = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAssignee", x => new { x.AccountId, x.EventDetailId });
                    table.ForeignKey(
                        name: "FK_EventAssignee_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventAssignee_EventDetail_EventDetailId",
                        column: x => x.EventDetailId,
                        principalTable: "EventDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAssignee_EventDetailId",
                table: "EventAssignee",
                column: "EventDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAssignee");

            migrationBuilder.DropColumn(
                name: "StallForSaleInventory",
                table: "EventDetail");

            migrationBuilder.AlterColumn<double>(
                name: "TicketPrice",
                table: "EventDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatorID",
                table: "Event",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OperatorID",
                table: "Event",
                column: "OperatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Account_OperatorID",
                table: "Event",
                column: "OperatorID",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
