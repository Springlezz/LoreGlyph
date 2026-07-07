using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoreGlyph.Migrations
{
    /// <inheritdoc />
    public partial class AvatarAndImageUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "Languages",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_path",
                table: "Languages");
        }
    }
}
