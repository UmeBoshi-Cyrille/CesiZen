using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CesiZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ArticleModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresentationImagePath",
                table: "Articles",
                newName: "ImagePath");

            migrationBuilder.AddColumn<string>(
                name: "Alternative",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alternative",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Articles",
                newName: "PresentationImagePath");
        }
    }
}
