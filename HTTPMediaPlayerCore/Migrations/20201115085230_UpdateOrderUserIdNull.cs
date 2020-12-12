using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class UpdateOrderUserIdNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Orders_vd_Users_UserId",
                table: "vd_Orders");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vd_Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Orders_vd_Users_UserId",
                table: "vd_Orders",
                column: "UserId",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Orders_vd_Users_UserId",
                table: "vd_Orders");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vd_Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Orders_vd_Users_UserId",
                table: "vd_Orders",
                column: "UserId",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
