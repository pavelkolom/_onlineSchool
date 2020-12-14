using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class IncreaseContactFormButtonText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactFormButtonText",
                table: "vd_Authors",
                type: "NVARCHAR(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(10)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactFormButtonText",
                table: "vd_Authors",
                type: "NVARCHAR(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldNullable: true);
        }
    }
}
