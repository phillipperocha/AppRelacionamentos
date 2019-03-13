using Microsoft.EntityFrameworkCore.Migrations;

namespace AppRelacionamentos.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Aqui ele vai criar uma tabela chamada Values
            migrationBuilder.CreateTable(
                name: "Values",

                // Aqui diz que ele criará duas colunas dentro dessa tabela
                columns: table => new
                {
                    // O Id usará o auto increment do SQLite
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    // E aqui diz que ela usará o ID como a chave primária da tabela
                    table.PrimaryKey("PK_Values", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Aqui ele apenas deletará a nossa tabela
            migrationBuilder.DropTable(
                name: "Values");
        }
    }
}
