using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BlogPosts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "BlogPosts",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "BlogPosts");
        }
    }
}
