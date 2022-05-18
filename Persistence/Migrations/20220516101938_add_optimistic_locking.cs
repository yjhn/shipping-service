using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_service.Persistence.Migrations
{
    public partial class add_optimistic_locking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Shipments",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Senders",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "PostMachines",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Couriers",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Senders");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "PostMachines");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Couriers");
        }
    }
}
