using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_service.Migrations
{
    public partial class removeNamefromCourierautogenerateIDs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Couriers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Couriers",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");
        }
    }
}
