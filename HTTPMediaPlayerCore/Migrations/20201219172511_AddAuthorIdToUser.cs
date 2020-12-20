using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AddAuthorIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Authors_vd_Users_UserId",
                table: "vd_Authors");

            migrationBuilder.DropIndex(
                name: "IX_vd_Authors_UserId",
                table: "vd_Authors");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAuthor",
                table: "vd_Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAdmin",
                table: "vd_Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "vd_Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactFormHeaderText",
                table: "vd_Authors",
                type: "NVARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactFormButtonText",
                table: "vd_Authors",
                type: "NVARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "vd_Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_vd_Authors_UserId1",
                table: "vd_Authors",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Authors_vd_Users_UserId1",
                table: "vd_Authors",
                column: "UserId1",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Authors_vd_Users_UserId1",
                table: "vd_Authors");

            migrationBuilder.DropIndex(
                name: "IX_vd_Authors_UserId1",
                table: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "vd_Authors");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAuthor",
                table: "vd_Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAdmin",
                table: "vd_Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactFormHeaderText",
                table: "vd_Authors",
                type: "NVARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactFormButtonText",
                table: "vd_Authors",
                type: "NVARCHAR(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_vd_Authors_UserId",
                table: "vd_Authors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Authors_vd_Users_UserId",
                table: "vd_Authors",
                column: "UserId",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
