using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AddRobokassa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasOwnRobokassa",
                table: "vd_Users",
                type: "bit",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasOwnRobokassa",
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
        }
    }
}
