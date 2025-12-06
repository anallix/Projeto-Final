using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDisponivelToMesa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Disponivel",
                table: "Mesas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disponivel",
                table: "Mesas");
        }
    }
}
