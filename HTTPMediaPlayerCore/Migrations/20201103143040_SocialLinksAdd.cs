using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class SocialLinksAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "vd_Users",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "vd_Users",
                type: "NVARCHAR(100)",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "VK",
                table: "vd_Users");

            migrationBuilder.DropColumn(
                name: "YouTube",
                table: "vd_Users");
        }
    }
}
