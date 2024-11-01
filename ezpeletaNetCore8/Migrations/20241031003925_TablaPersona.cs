using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class TablaPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.AddColumn<int>(
                name: "PersonaID",
                table: "Lugares",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LugarID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PersonaID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_PersonaID",
                table: "Lugares",
                column: "PersonaID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_PersonaID",
                table: "EjerciciosFisicos",
                column: "PersonaID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID",
                principalTable: "Lugares",
                principalColumn: "LugarID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Personas_PersonaID",
                table: "EjerciciosFisicos",
                column: "PersonaID",
                principalTable: "Personas",
                principalColumn: "PersonaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_Personas_PersonaID",
                table: "Lugares",
                column: "PersonaID",
                principalTable: "Personas",
                principalColumn: "PersonaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Personas_PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_Personas_PersonaID",
                table: "Lugares");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Lugares_PersonaID",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "PersonaID",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "PersonaID",
                table: "EjerciciosFisicos");

            migrationBuilder.AlterColumn<int>(
                name: "LugarID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID",
                principalTable: "Lugares",
                principalColumn: "LugarID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
