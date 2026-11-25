using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Students : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningPlans_ClassStudieds_ClassStudiedId",
                table: "LearningPlans");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "LearningPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "LearningPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassStudiedId",
                table: "LearningPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initial_Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    degreeType = table.Column<int>(type: "int", nullable: false),
                    relationshipType = table.Column<int>(type: "int", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CitizenCardAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Workplace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosePersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosePersonPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MedicalCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LineId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    adviser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassStudie_Id = table.Column<int>(type: "int", nullable: true),
                    month1 = table.Column<double>(type: "float", nullable: false),
                    month2 = table.Column<double>(type: "float", nullable: false),
                    month3 = table.Column<double>(type: "float", nullable: false),
                    month4 = table.Column<double>(type: "float", nullable: false),
                    month5 = table.Column<double>(type: "float", nullable: false),
                    month6 = table.Column<double>(type: "float", nullable: false),
                    Attendance_Score = table.Column<double>(type: "float", nullable: true),
                    credit = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_students_ClassStudieds_ClassStudie_Id",
                        column: x => x.ClassStudie_Id,
                        principalTable: "ClassStudieds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_students_Initials_Initial_Id",
                        column: x => x.Initial_Id,
                        principalTable: "Initials",
                        principalColumn: "Initial_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassStudie_Id",
                table: "students",
                column: "ClassStudie_Id");

            migrationBuilder.CreateIndex(
                name: "IX_students_Initial_Id",
                table: "students",
                column: "Initial_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningPlans_ClassStudieds_ClassStudiedId",
                table: "LearningPlans",
                column: "ClassStudiedId",
                principalTable: "ClassStudieds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningPlans_ClassStudieds_ClassStudiedId",
                table: "LearningPlans");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "LearningPlans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "LearningPlans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ClassStudiedId",
                table: "LearningPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningPlans_ClassStudieds_ClassStudiedId",
                table: "LearningPlans",
                column: "ClassStudiedId",
                principalTable: "ClassStudieds",
                principalColumn: "Id");
        }
    }
}
