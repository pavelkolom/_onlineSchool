using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AddIsPayingRobokassaFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPayingRobokassaFee",
                table: "vd_Authors",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayingRobokassaFee",
                table: "vd_Authors");
        }
    }
}
