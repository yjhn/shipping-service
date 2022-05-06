using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_service.Persistence.Migrations
{
    public partial class add_unlock_codes_to_shipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestPmCourierUnlockCode",
                table: "Shipments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestPmSenderUnlockCode",
                table: "Shipments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SrcPmCourierUnlockCode",
                table: "Shipments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SrcPmSenderUnlockCode",
                table: "Shipments",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestPmCourierUnlockCode",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DestPmSenderUnlockCode",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "SrcPmCourierUnlockCode",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "SrcPmSenderUnlockCode",
                table: "Shipments");
        }
    }
}
