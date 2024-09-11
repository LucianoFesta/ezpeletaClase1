using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ezpeletaNetCore8.Migrations
{
    /// <inheritdoc />
    public partial class TablaLugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LugarID",
                table: "EjerciciosFisicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lugares",
                columns: table => new
                {
                    LugarID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lugares", x => x.LugarID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EjerciciosFisicos_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID");

            migrationBuilder.AddForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos",
                column: "LugarID",
                principalTable: "Lugares",
                principalColumn: "LugarID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EjerciciosFisicos_Lugares_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropTable(
                name: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_EjerciciosFisicos_LugarID",
                table: "EjerciciosFisicos");

            migrationBuilder.DropColumn(
                name: "LugarID",
                table: "EjerciciosFisicos");
        }
    }
}
