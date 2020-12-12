using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class UpdateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Orders_vd_Courses_CourseId",
                table: "vd_Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "vd_Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "vd_Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "vd_Orders",
                type: "NVARCHAR(200)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Orders_vd_Courses_CourseId",
                table: "vd_Orders",
                column: "CourseId",
                principalTable: "vd_Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Orders_vd_Courses_CourseId",
                table: "vd_Orders");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "vd_Orders");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "vd_Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "vd_Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Orders_vd_Courses_CourseId",
                table: "vd_Orders",
                column: "CourseId",
                principalTable: "vd_Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
