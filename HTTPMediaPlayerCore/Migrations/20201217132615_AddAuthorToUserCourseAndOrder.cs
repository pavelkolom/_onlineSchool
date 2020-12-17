using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AddAuthorToUserCourseAndOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "vd_UserCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "vd_Orders",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "vd_UserCourses");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "vd_Orders");
        }
    }
}
