using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoreGlyph.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageSharing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShareToken",
                table: "Languages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "link",
                table: "Languages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareToken",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "link",
                table: "Languages");
        }
    }
}
