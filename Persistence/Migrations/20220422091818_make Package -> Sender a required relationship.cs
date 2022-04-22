using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shipping_service.Persistence.Migrations
{
    public partial class makePackageSenderarequiredrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Senders_SenderId",
                table: "Packages");

            migrationBuilder.AlterColumn<decimal>(
                name: "SenderId",
                table: "Packages",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Senders_SenderId",
                table: "Packages",
                column: "SenderId",
                principalTable: "Senders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Senders_SenderId",
                table: "Packages");

            migrationBuilder.AlterColumn<decimal>(
                name: "SenderId",
                table: "Packages",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Senders_SenderId",
                table: "Packages",
                column: "SenderId",
                principalTable: "Senders",
                principalColumn: "Id");
        }
    }
}
