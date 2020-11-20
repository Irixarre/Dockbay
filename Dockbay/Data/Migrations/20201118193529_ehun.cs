using Microsoft.EntityFrameworkCore.Migrations;

namespace Dockbay.Data.Migrations
{
    public partial class ehun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Disponible",
                table: "Dock",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "Dock");
        }
    }
}
