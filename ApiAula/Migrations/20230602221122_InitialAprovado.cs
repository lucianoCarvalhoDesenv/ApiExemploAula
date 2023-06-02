using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAula.Migrations
{
    public partial class InitialAprovado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstaAprovado",
                table: "Alunos",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstaAprovado",
                table: "Alunos");
        }
    }
}
