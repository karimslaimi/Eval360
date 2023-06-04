using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class qualiteEvalMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "qualiteEvaluateur",
                table: "Compagnie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qualiteEvaluateur",
                table: "Compagnie");
        }
    }
}
