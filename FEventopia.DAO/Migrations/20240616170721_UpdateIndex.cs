using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FEventopia.DAO.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ticket_EventDetailID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TransactionID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_SponsorEvent_EventID",
                table: "SponsorEvent");

            migrationBuilder.DropIndex(
                name: "IX_SponsorEvent_TransactionID",
                table: "SponsorEvent");

            migrationBuilder.DropIndex(
                name: "IX_EventStall_EventDetailID",
                table: "EventStall");

            migrationBuilder.DropIndex(
                name: "IX_EventStall_TransactionID",
                table: "EventStall");

            migrationBuilder.AlterColumn<double>(
                name: "TicketPrice",
                table: "EventDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EstimateCost",
                table: "EventDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventDetailID",
                table: "Ticket",
                column: "EventDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TransactionID",
                table: "Ticket",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorManagement_EventId",
                table: "SponsorManagement",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorEvent_EventID",
                table: "SponsorEvent",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_SponsorEvent_TransactionID",
                table: "SponsorEvent",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_EventStall_EventDetailID",
                table: "EventStall",
                column: "EventDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_EventStall_TransactionID",
                table: "EventStall",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_EventAssignee_AccountId",
                table: "EventAssignee",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ticket_EventDetailID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TransactionID",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_SponsorManagement_EventId",
                table: "SponsorManagement");

            migrationBuilder.DropIndex(
                name: "IX_SponsorEvent_EventID",
                table: "SponsorEvent");

            migrationBuilder.DropIndex(
                name: "IX_SponsorEvent_TransactionID",
                table: "SponsorEvent");

            migrationBuilder.DropIndex(
                name: "IX_EventStall_EventDetailID",
                table: "EventStall");

            migrationBuilder.DropIndex(
                name: "IX_EventStall_TransactionID",
                table: "EventStall");

            migrationBuilder.DropIndex(
                name: "IX_EventAssignee_AccountId",
                table: "EventAssignee");

            migrationBuilder.AlterColumn<double>(
                name: "TicketPrice",
                table: "EventDetail",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "EstimateCost",
                table: "EventDetail",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventDetailID",
                table: "Ticket",
                column: "EventDetailID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TransactionID",
                table: "Ticket",
                column: "TransactionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SponsorEvent_EventID",
                table: "SponsorEvent",
                column: "EventID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SponsorEvent_TransactionID",
                table: "SponsorEvent",
                column: "TransactionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventStall_EventDetailID",
                table: "EventStall",
                column: "EventDetailID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventStall_TransactionID",
                table: "EventStall",
                column: "TransactionID",
                unique: true);
        }
    }
}
