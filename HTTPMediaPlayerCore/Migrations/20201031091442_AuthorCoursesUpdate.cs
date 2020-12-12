using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AuthorCoursesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vd_CourseAuthors");

            migrationBuilder.CreateTable(
                name: "vd_AuthorCourses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_AuthorCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_AuthorCourses_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_AuthorCourses_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_AuthorCourses_CourseId",
                table: "vd_AuthorCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_AuthorCourses_UserId",
                table: "vd_AuthorCourses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vd_AuthorCourses");

            migrationBuilder.CreateTable(
                name: "vd_CourseAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_CourseAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_CourseAuthors_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_CourseAuthors_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseAuthors_CourseId",
                table: "vd_CourseAuthors",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseAuthors_UserId",
                table: "vd_CourseAuthors",
                column: "UserId");
        }
    }
}
