using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class compagnieReponse2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "note",
                table: "CompagnieReponse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note",
                table: "CompagnieReponse");
        }
    }
}
