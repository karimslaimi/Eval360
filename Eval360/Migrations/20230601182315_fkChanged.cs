using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class fkChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_superiorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Poste_PosteId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Compagnie_AspNetUsers_employeeId",
                table: "Compagnie");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieQuestions_Compagnie_compagnieid",
                table: "CompagnieQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieQuestions_Question_questionid",
                table: "CompagnieQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_CompagnieQuestionid",
                table: "CompagnieReponse");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieUser_Compagnie_compagnieid",
                table: "CompagnieUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_AxeEval_axeEvalid",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PosteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_superiorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PosteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "superiorId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "axeEvalid",
                table: "Question",
                newName: "idEval");

            migrationBuilder.RenameIndex(
                name: "IX_Question_axeEvalid",
                table: "Question",
                newName: "IX_Question_idEval");

            migrationBuilder.RenameColumn(
                name: "compagnieid",
                table: "CompagnieUser",
                newName: "idCompagnie");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieUser_compagnieid",
                table: "CompagnieUser",
                newName: "IX_CompagnieUser_idCompagnie");

            migrationBuilder.RenameColumn(
                name: "CompagnieQuestionid",
                table: "CompagnieReponse",
                newName: "compagnieQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieReponse_CompagnieQuestionid",
                table: "CompagnieReponse",
                newName: "IX_CompagnieReponse_compagnieQuestionId");

            migrationBuilder.RenameColumn(
                name: "questionid",
                table: "CompagnieQuestions",
                newName: "questionId");

            migrationBuilder.RenameColumn(
                name: "compagnieid",
                table: "CompagnieQuestions",
                newName: "compagnieId");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieQuestions_questionid",
                table: "CompagnieQuestions",
                newName: "IX_CompagnieQuestions_questionId");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieQuestions_compagnieid",
                table: "CompagnieQuestions",
                newName: "IX_CompagnieQuestions_compagnieId");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "Compagnie",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Compagnie_employeeId",
                table: "Compagnie",
                newName: "IX_Compagnie_userId");

            migrationBuilder.AlterColumn<string>(
                name: "idSuperior",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idPoste",
                table: "AspNetUsers",
                column: "idPoste");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_idSuperior",
                table: "AspNetUsers",
                column: "idSuperior");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_idSuperior",
                table: "AspNetUsers",
                column: "idSuperior",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Poste_idPoste",
                table: "AspNetUsers",
                column: "idPoste",
                principalTable: "Poste",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compagnie_AspNetUsers_userId",
                table: "Compagnie",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieQuestions_Compagnie_compagnieId",
                table: "CompagnieQuestions",
                column: "compagnieId",
                principalTable: "Compagnie",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieQuestions_Question_questionId",
                table: "CompagnieQuestions",
                column: "questionId",
                principalTable: "Question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieReponse",
                column: "compagnieQuestionId",
                principalTable: "CompagnieQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieUser_Compagnie_idCompagnie",
                table: "CompagnieUser",
                column: "idCompagnie",
                principalTable: "Compagnie",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AxeEval_idEval",
                table: "Question",
                column: "idEval",
                principalTable: "AxeEval",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_idSuperior",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Poste_idPoste",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Compagnie_AspNetUsers_userId",
                table: "Compagnie");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieQuestions_Compagnie_compagnieId",
                table: "CompagnieQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieQuestions_Question_questionId",
                table: "CompagnieQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieReponse");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieUser_Compagnie_idCompagnie",
                table: "CompagnieUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_AxeEval_idEval",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_idPoste",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_idSuperior",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "idEval",
                table: "Question",
                newName: "axeEvalid");

            migrationBuilder.RenameIndex(
                name: "IX_Question_idEval",
                table: "Question",
                newName: "IX_Question_axeEvalid");

            migrationBuilder.RenameColumn(
                name: "idCompagnie",
                table: "CompagnieUser",
                newName: "compagnieid");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieUser_idCompagnie",
                table: "CompagnieUser",
                newName: "IX_CompagnieUser_compagnieid");

            migrationBuilder.RenameColumn(
                name: "compagnieQuestionId",
                table: "CompagnieReponse",
                newName: "CompagnieQuestionid");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieReponse_compagnieQuestionId",
                table: "CompagnieReponse",
                newName: "IX_CompagnieReponse_CompagnieQuestionid");

            migrationBuilder.RenameColumn(
                name: "questionId",
                table: "CompagnieQuestions",
                newName: "questionid");

            migrationBuilder.RenameColumn(
                name: "compagnieId",
                table: "CompagnieQuestions",
                newName: "compagnieid");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieQuestions_questionId",
                table: "CompagnieQuestions",
                newName: "IX_CompagnieQuestions_questionid");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieQuestions_compagnieId",
                table: "CompagnieQuestions",
                newName: "IX_CompagnieQuestions_compagnieid");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Compagnie",
                newName: "employeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Compagnie_userId",
                table: "Compagnie",
                newName: "IX_Compagnie_employeeId");

            migrationBuilder.AlterColumn<string>(
                name: "idSuperior",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PosteId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "superiorId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PosteId",
                table: "AspNetUsers",
                column: "PosteId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_superiorId",
                table: "AspNetUsers",
                column: "superiorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_superiorId",
                table: "AspNetUsers",
                column: "superiorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Poste_PosteId",
                table: "AspNetUsers",
                column: "PosteId",
                principalTable: "Poste",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Compagnie_AspNetUsers_employeeId",
                table: "Compagnie",
                column: "employeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieQuestions_Compagnie_compagnieid",
                table: "CompagnieQuestions",
                column: "compagnieid",
                principalTable: "Compagnie",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieQuestions_Question_questionid",
                table: "CompagnieQuestions",
                column: "questionid",
                principalTable: "Question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_CompagnieQuestionid",
                table: "CompagnieReponse",
                column: "CompagnieQuestionid",
                principalTable: "CompagnieQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieUser_Compagnie_compagnieid",
                table: "CompagnieUser",
                column: "compagnieid",
                principalTable: "Compagnie",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_AxeEval_axeEvalid",
                table: "Question",
                column: "axeEvalid",
                principalTable: "AxeEval",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
