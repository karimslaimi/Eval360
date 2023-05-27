using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class newMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "niveau",
                table: "Poste");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "niveau",
                table: "Poste",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
