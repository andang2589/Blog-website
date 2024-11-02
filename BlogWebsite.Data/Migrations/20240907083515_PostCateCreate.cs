using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostCateCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPostCategory",
                columns: table => new
                {
                    CategoriesCategoryID = table.Column<int>(type: "int", nullable: false),
                    PostsBlogPostID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostCategory", x => new { x.CategoriesCategoryID, x.PostsBlogPostID });
                    table.ForeignKey(
                        name: "FK_BlogPostCategory_BlogPosts_PostsBlogPostID",
                        column: x => x.PostsBlogPostID,
                        principalTable: "BlogPosts",
                        principalColumn: "BlogPostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostCategory_Categories_CategoriesCategoryID",
                        column: x => x.CategoriesCategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostCategory_PostsBlogPostID",
                table: "BlogPostCategory",
                column: "PostsBlogPostID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostCategory");
        }
    }
}
