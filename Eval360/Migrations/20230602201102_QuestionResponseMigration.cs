using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Migrations
{
    public partial class QuestionResponseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieReponse_AspNetUsers_userId",
                table: "CompagnieReponse");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieReponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompagnieReponse",
                table: "CompagnieReponse");

            migrationBuilder.RenameTable(
                name: "CompagnieReponse",
                newName: "CompagnieResponse");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieReponse_userId",
                table: "CompagnieResponse",
                newName: "IX_CompagnieResponse_userId");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieReponse_compagnieQuestionId",
                table: "CompagnieResponse",
                newName: "IX_CompagnieResponse_compagnieQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompagnieResponse",
                table: "CompagnieResponse",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieResponse_AspNetUsers_userId",
                table: "CompagnieResponse",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieResponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieResponse",
                column: "compagnieQuestionId",
                principalTable: "CompagnieQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieResponse_AspNetUsers_userId",
                table: "CompagnieResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_CompagnieResponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompagnieResponse",
                table: "CompagnieResponse");

            migrationBuilder.RenameTable(
                name: "CompagnieResponse",
                newName: "CompagnieReponse");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieResponse_userId",
                table: "CompagnieReponse",
                newName: "IX_CompagnieReponse_userId");

            migrationBuilder.RenameIndex(
                name: "IX_CompagnieResponse_compagnieQuestionId",
                table: "CompagnieReponse",
                newName: "IX_CompagnieReponse_compagnieQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompagnieReponse",
                table: "CompagnieReponse",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieReponse_AspNetUsers_userId",
                table: "CompagnieReponse",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompagnieReponse_CompagnieQuestions_compagnieQuestionId",
                table: "CompagnieReponse",
                column: "compagnieQuestionId",
                principalTable: "CompagnieQuestions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
