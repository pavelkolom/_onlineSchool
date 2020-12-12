using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class CourseAndUserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "vd_Users",
                type: "NVARCHAR(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPageHeaderPic",
                table: "vd_Users",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPageSlogan",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPageTitle",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonNumber",
                table: "vd_Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "PersonalPageHeaderPic",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "PersonalPageSlogan",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "PersonalPageTitle",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "LessonNumber",
                table: "vd_Courses");
        }
    }
}
