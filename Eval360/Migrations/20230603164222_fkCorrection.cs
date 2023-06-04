using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class fkCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_AxeEval_idEval",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "idEval",
                table: "Question",
                newName: "idAxe");

            migrationBuilder.RenameIndex(
                name: "IX_Question_idEval",
                table: "Question",
                newName: "IX_Question_idAxe");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AxeEval_idAxe",
                table: "Question",
                column: "idAxe",
                principalTable: "AxeEval",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_AxeEval_idAxe",
                table: "Question");

            migrationBuilder.RenameColumn(
                name: "idAxe",
                table: "Question",
                newName: "idEval");

            migrationBuilder.RenameIndex(
                name: "IX_Question_idAxe",
                table: "Question",
                newName: "IX_Question_idEval");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AxeEval_idEval",
                table: "Question",
                column: "idEval",
                principalTable: "AxeEval",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
