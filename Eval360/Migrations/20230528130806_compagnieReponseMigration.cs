using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class compagnieReponseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "employeeId",
                table: "Compagnie",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CompagnieReponse",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompagnieQuestionid = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompagnieReponse", x => x.id);
                    table.ForeignKey(
                        name: "FK_CompagnieReponse_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompagnieReponse_CompagnieQuestions_CompagnieQuestionid",
                        column: x => x.CompagnieQuestionid,
                        principalTable: "CompagnieQuestions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompagnieReponse_CompagnieQuestionid",
                table: "CompagnieReponse",
                column: "CompagnieQuestionid");

            migrationBuilder.CreateIndex(
                name: "IX_CompagnieReponse_userId",
                table: "CompagnieReponse",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompagnieReponse");

            migrationBuilder.AlterColumn<string>(
                name: "employeeId",
                table: "Compagnie",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
