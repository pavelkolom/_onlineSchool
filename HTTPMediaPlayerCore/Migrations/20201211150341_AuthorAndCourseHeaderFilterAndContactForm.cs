using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class AuthorAndCourseHeaderFilterAndContactForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursePageHeaderFilter",
                table: "vd_Courses");

            migrationBuilder.RenameColumn(
                name: "PersonalPageHeaderFilter",
                table: "vd_Authors",
                newName: "ContactFormHeaderText");

            migrationBuilder.AddColumn<int>(
                name: "FilterId",
                table: "vd_Courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactFormButtonText",
                table: "vd_Authors",
                type: "NVARCHAR(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactFormMessageBoxText",
                table: "vd_Authors",
                type: "NVARCHAR(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FilterId",
                table: "vd_Authors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasContactForm",
                table: "vd_Authors",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vd_Filters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Filters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_Courses_FilterId",
                table: "vd_Courses",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Authors_FilterId",
                table: "vd_Authors",
                column: "FilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Authors_vd_Filters_FilterId",
                table: "vd_Authors",
                column: "FilterId",
                principalTable: "vd_Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Courses_vd_Filters_FilterId",
                table: "vd_Courses",
                column: "FilterId",
                principalTable: "vd_Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Authors_vd_Filters_FilterId",
                table: "vd_Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_vd_Courses_vd_Filters_FilterId",
                table: "vd_Courses");

            migrationBuilder.DropTable(
                name: "vd_Filters");

            migrationBuilder.DropIndex(
                name: "IX_vd_Courses_FilterId",
                table: "vd_Courses");

            migrationBuilder.DropIndex(
                name: "IX_vd_Authors_FilterId",
                table: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "FilterId",
                table: "vd_Courses");

            migrationBuilder.DropColumn(
                name: "ContactFormButtonText",
                table: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "ContactFormMessageBoxText",
                table: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "FilterId",
                table: "vd_Authors");

            migrationBuilder.DropColumn(
                name: "HasContactForm",
                table: "vd_Authors");

            migrationBuilder.RenameColumn(
                name: "ContactFormHeaderText",
                table: "vd_Authors",
                newName: "PersonalPageHeaderFilter");

            migrationBuilder.AddColumn<string>(
                name: "CoursePageHeaderFilter",
                table: "vd_Courses",
                type: "NVARCHAR(20)",
                nullable: true);
        }
    }
}
