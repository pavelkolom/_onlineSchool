using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class ConnectAuthorToAuthorCources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_vd_AuthorCourses_AuthorId",
                table: "vd_AuthorCourses",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_AuthorCourses_vd_Authors_AuthorId",
                table: "vd_AuthorCourses",
                column: "AuthorId",
                principalTable: "vd_Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_AuthorCourses_vd_Authors_AuthorId",
                table: "vd_AuthorCourses");

            migrationBuilder.DropIndex(
                name: "IX_vd_AuthorCourses_AuthorId",
                table: "vd_AuthorCourses");
        }
    }
}
