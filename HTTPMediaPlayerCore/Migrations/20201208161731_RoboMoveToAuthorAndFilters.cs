using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class RoboMoveToAuthorAndFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "HasOwnRobokassa",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "Instagram",
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
                name: "RobokassaPassword1",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "RobokassaPassword2",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "RobokassaShopId",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "RobokassaTestPassword1",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "RobokassaTestPassword2",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "VK",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "YouTube",
                table: "vd_Users");

            migrationBuilder.AddColumn<string>(
                name: "CoursePageHeaderFilter",
                table: "vd_Courses",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalPageHeaderFilter",
                table: "vd_Authors",
                type: "NVARCHAR(20)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursePageHeaderFilter",
                table: "vd_Courses");

            migrationBuilder.DropColumn(
                name: "PersonalPageHeaderFilter",
                table: "vd_Authors");

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOwnRobokassa",
                table: "vd_Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "vd_Users",
                type: "NVARCHAR(100)",
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
                name: "RobokassaPassword1",
                table: "vd_Users",
                type: "NVARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobokassaPassword2",
                table: "vd_Users",
                type: "NVARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobokassaShopId",
                table: "vd_Users",
                type: "NVARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobokassaTestPassword1",
                table: "vd_Users",
                type: "NVARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobokassaTestPassword2",
                table: "vd_Users",
                type: "NVARCHAR(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VK",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTube",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);
        }
    }
}
