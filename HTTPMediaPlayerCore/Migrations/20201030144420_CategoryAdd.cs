using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class CategoryAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "vd_Courses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vd_Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_Courses_CategoryId",
                table: "vd_Courses",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_vd_Courses_vd_Category_CategoryId",
                table: "vd_Courses",
                column: "CategoryId",
                principalTable: "vd_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vd_Courses_vd_Category_CategoryId",
                table: "vd_Courses");

            migrationBuilder.DropTable(
                name: "vd_Category");

            migrationBuilder.DropIndex(
                name: "IX_vd_Courses_CategoryId",
                table: "vd_Courses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "vd_Courses");
        }
    }
}
