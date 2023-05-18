using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eval360.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "cin",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateEmbauche",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "preNom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sexe",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Direction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Poste",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    niveau = table.Column<int>(type: "int", nullable: false),
                    IdDirection = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poste", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poste_Direction_IdDirection",
                        column: x => x.IdDirection,
                        principalTable: "Direction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poste_IdDirection",
                table: "Poste",
                column: "IdDirection");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poste");

            migrationBuilder.DropTable(
                name: "Direction");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "cin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "dateEmbauche",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "preNom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "sexe",
                table: "AspNetUsers");
        }
    }
}
