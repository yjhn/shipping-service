using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_service.Persistence.Migrations
{
    public partial class DestPmReceiverUnlockCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestPmSenderUnlockCode",
                table: "Shipments",
                newName: "DestPmReceiverUnlockCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestPmReceiverUnlockCode",
                table: "Shipments",
                newName: "DestPmSenderUnlockCode");
        }
    }
}
