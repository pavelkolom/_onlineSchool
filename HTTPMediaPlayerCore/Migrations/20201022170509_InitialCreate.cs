using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTPMediaPlayerCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vd_ActionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_ActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vd_Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RusName = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    EngName = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    Alpha2 = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    Alpha3 = table.Column<string>(type: "NVARCHAR(10)", nullable: true),
                    ISO = table.Column<string>(type: "NVARCHAR(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vd_Courses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    UrlName = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(4000)", nullable: false),
                    ShortDescription = table.Column<string>(type: "NVARCHAR(400)", nullable: true),
                    OrderNumber = table.Column<int>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsForBeginners = table.Column<bool>(nullable: true),
                    IsForIntermediate = table.Column<bool>(nullable: true),
                    IsForAdvanced = table.Column<bool>(nullable: true),
                    IsForKids = table.Column<bool>(nullable: true),
                    IsForAdults = table.Column<bool>(nullable: true),
                    FullCourseId = table.Column<int>(nullable: true),
                    ForWhom = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    Contents = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    Info = table.Column<string>(type: "NVARCHAR(4000)", nullable: true),
                    HasTrial = table.Column<bool>(nullable: true),
                    IsBook = table.Column<bool>(nullable: true),
                    HasDownloadItem = table.Column<bool>(nullable: true),
                    PriceDownloadItem = table.Column<int>(nullable: true),
                    PageText = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    IsLanguageCourse = table.Column<bool>(nullable: true),
                    IsWritingCourse = table.Column<bool>(nullable: true),
                    IsPackage = table.Column<bool>(nullable: true),
                    ParentCourseId = table.Column<int>(nullable: true),
                    HasPackages = table.Column<bool>(nullable: true),
                    ActivationDate = table.Column<DateTime>(nullable: true),
                    ActivationFreq = table.Column<int>(nullable: true),
                    AuthorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vd_Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Url = table.Column<string>(type: "NVARCHAR(1000)", nullable: true),
                    Ext = table.Column<string>(type: "NVARCHAR(4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vd_Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UrlName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    LoginName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    IsAuthor = table.Column<bool>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    City = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    IsActivated = table.Column<bool>(nullable: false),
                    Token = table.Column<Guid>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: true),
                    Phone = table.Column<string>(type: "NVARCHAR(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_Users_vd_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "vd_Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vd_Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(1000)", nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<int>(nullable: true),
                    Text = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_Lessons_vd_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "vd_Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_ActionLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    ActionTypeId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_ActionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_ActionLog_vd_ActionType_ActionTypeId",
                        column: x => x.ActionTypeId,
                        principalTable: "vd_ActionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_ActionLog_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_CourseAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "vd_Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    LoginDT = table.Column<DateTime>(nullable: false),
                    LogoutDT = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_Log_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_UserCourses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActivated = table.Column<bool>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: true),
                    SubscriptionDate = table.Column<DateTime>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_UserCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_UserCourses_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_UserCourses_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_CourseLessons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(nullable: false),
                    IsForTrial = table.Column<bool>(nullable: true),
                    LessonId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_CourseLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_CourseLessons_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_CourseLessons_vd_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "vd_Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_UserLessons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<Guid>(nullable: false),
                    ActivationDate = table.Column<DateTime>(nullable: true),
                    IsReadByUser = table.Column<bool>(nullable: true),
                    IsLinkSent = table.Column<bool>(nullable: true),
                    LessonId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_UserLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_UserLessons_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_UserLessons_vd_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "vd_Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_UserLessons_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vd_Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    PaymentDateTime = table.Column<DateTime>(nullable: true),
                    ResultResponse = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Sum = table.Column<decimal>(nullable: false),
                    IncSum = table.Column<decimal>(nullable: true),
                    Fee = table.Column<decimal>(nullable: true),
                    SignatureValue = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    IsTest = table.Column<bool>(nullable: true),
                    EMail = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserCourseId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vd_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vd_Orders_vd_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "vd_Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vd_Orders_vd_UserCourses_UserCourseId",
                        column: x => x.UserCourseId,
                        principalTable: "vd_UserCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vd_Orders_vd_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "vd_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vd_ActionLog_ActionTypeId",
                table: "vd_ActionLog",
                column: "ActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_ActionLog_UserId",
                table: "vd_ActionLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseAuthors_CourseId",
                table: "vd_CourseAuthors",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseAuthors_UserId",
                table: "vd_CourseAuthors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseLessons_CourseId",
                table: "vd_CourseLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_CourseLessons_LessonId",
                table: "vd_CourseLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Lessons_FileId",
                table: "vd_Lessons",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Log_UserId",
                table: "vd_Log",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Orders_CourseId",
                table: "vd_Orders",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Orders_UserCourseId",
                table: "vd_Orders",
                column: "UserCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Orders_UserId",
                table: "vd_Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_UserCourses_CourseId",
                table: "vd_UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_UserCourses_UserId",
                table: "vd_UserCourses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_UserLessons_CourseId",
                table: "vd_UserLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_UserLessons_LessonId",
                table: "vd_UserLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_UserLessons_UserId",
                table: "vd_UserLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_vd_Users_CountryId",
                table: "vd_Users",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vd_ActionLog");

            migrationBuilder.DropTable(
                name: "vd_CourseAuthors");

            migrationBuilder.DropTable(
                name: "vd_CourseLessons");

            migrationBuilder.DropTable(
                name: "vd_Log");

            migrationBuilder.DropTable(
                name: "vd_Orders");

            migrationBuilder.DropTable(
                name: "vd_UserLessons");

            migrationBuilder.DropTable(
                name: "vd_ActionType");

            migrationBuilder.DropTable(
                name: "vd_UserCourses");

            migrationBuilder.DropTable(
                name: "vd_Lessons");

            migrationBuilder.DropTable(
                name: "vd_Courses");

            migrationBuilder.DropTable(
                name: "vd_Users");

            migrationBuilder.DropTable(
                name: "vd_Files");

            migrationBuilder.DropTable(
                name: "vd_Countries");
        }
    }
}
