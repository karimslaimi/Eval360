using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class EvalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AxeEval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AxeEval", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Compagnie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    employeeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compagnie", x => x.id);
                    table.ForeignKey(
                        name: "FK_Compagnie_AspNetUsers_employeeId",
                        column: x => x.employeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isEnabled = table.Column<bool>(type: "bit", nullable: false),
                    axeEvalid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.id);
                    table.ForeignKey(
                        name: "FK_Question_AxeEval_axeEvalid",
                        column: x => x.axeEvalid,
                        principalTable: "AxeEval",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompagnieQuestions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    compagnieid = table.Column<int>(type: "int", nullable: false),
                    questionid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompagnieQuestions", x => x.id);
                    table.ForeignKey(
                        name: "FK_CompagnieQuestions_Compagnie_compagnieid",
                        column: x => x.compagnieid,
                        principalTable: "Compagnie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompagnieQuestions_Question_questionid",
                        column: x => x.questionid,
                        principalTable: "Question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compagnie_employeeId",
                table: "Compagnie",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompagnieQuestions_compagnieid",
                table: "CompagnieQuestions",
                column: "compagnieid");

            migrationBuilder.CreateIndex(
                name: "IX_CompagnieQuestions_questionid",
                table: "CompagnieQuestions",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "IX_Question_axeEvalid",
                table: "Question",
                column: "axeEvalid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompagnieQuestions");

            migrationBuilder.DropTable(
                name: "Compagnie");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "AxeEval");
        }
    }
}
