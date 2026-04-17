using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roupa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarOpcoesLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Opcoes",
                table: "Layouts",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opcoes",
                table: "Layouts");
        }
    }
}
