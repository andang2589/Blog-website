using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommentTableNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentDate",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "Comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateModified",
                table: "Comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Comments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Comments",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParentID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentID",
                table: "Comments",
                column: "ParentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentID",
                table: "Comments",
                column: "ParentID",
                principalTable: "Comments",
                principalColumn: "CommentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
