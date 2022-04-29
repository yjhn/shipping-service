using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace shipping_service.Persistence.Migrations
{
    public partial class create_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Shipments",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Senders",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "PostMachines",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Couriers",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Shipments",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Senders",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "PostMachines",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Couriers",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);
        }
    }
}
