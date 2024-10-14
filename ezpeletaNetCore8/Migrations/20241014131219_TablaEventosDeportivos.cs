using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class TablaEventosDeportivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoEventoID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoEventoID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "EventoDeportivoEventoID",
                table: "EjerciciosFisicos");

            migrationBuilder.RenameColumn(
                name: "EventoID",
                table: "EjerciciosFisicos",
                newName: "EventoDeportivoID");

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoID",
                principalTable: "EventosDeportivos",
                principalColumn: "EventoID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoID",
                table: "EjerciciosFisicos");

            migrationBuilder.RenameColumn(
                name: "EventoDeportivoID",
                table: "EjerciciosFisicos",
                newName: "EventoID");

            migrationBuilder.AddColumn<int>(
                name: "EventoDeportivoEventoID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_EventoDeportivoEventoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoEventoID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_EventosDeportivos_EventoDeportivoEventoID",
                table: "EjerciciosFisicos",
                column: "EventoDeportivoEventoID",
                principalTable: "EventosDeportivos",
                principalColumn: "EventoID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
