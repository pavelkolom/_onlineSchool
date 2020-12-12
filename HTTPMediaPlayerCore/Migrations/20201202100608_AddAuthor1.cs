using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AddAuthor1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_AuthorCourses_vd_Users_UserId",
                table: "vd_AuthorCourses");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vd_AuthorCourses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "vd_AuthorCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "vd_Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorUrl = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    WorkShopName = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    PersonalPageTitle = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PersonalPageSlogan = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    PersonalPageHeaderPic = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    PersonalPageHTML = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Instagram = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    Facebook = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    VK = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    YouTube = table.Column<string>(type: "NVARCHAR(100)", nullable: true),
                    HasOwnRobokassa = table.Column<bool>(type: "bit", nullable: true),
                    RobokassaShopId = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    RobokassaPassword1 = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    RobokassaPassword2 = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    RobokassaTestPassword1 = table.Column<string>(type: "NVARCHAR(30)", nullable: true),
                    RobokassaTestPassword2 = table.Column<string>(type: "NVARCHAR(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_Authors_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_Authors_UserId",
                table: "vd_Authors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_AuthorCourses_vd_Users_UserId",
                table: "vd_AuthorCourses",
                column: "UserId",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_AuthorCourses_vd_Users_UserId",
                table: "vd_AuthorCourses");

            migrationBuilder.DropTable(
                name: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "vd_AuthorCourses");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "vd_AuthorCourses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vd_AuthorCourses_vd_Users_UserId",
                table: "vd_AuthorCourses",
                column: "UserId",
                principalTable: "vd_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
