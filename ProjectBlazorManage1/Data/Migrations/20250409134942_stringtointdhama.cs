using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class stringtointdhama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseNames_Teachers_Teacher_Id",
                table: "CourseNames");

            migrationBuilder.DropIndex(
                name: "IX_CourseNames_Teacher_Id",
                table: "CourseNames");

            migrationBuilder.DropColumn(
                name: "Teacher_Id",
                table: "CourseNames");

            migrationBuilder.AlterColumn<int>(
                name: "generation",
                table: "DhammaCourses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "generation",
                table: "DhammaCourses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Teacher_Id",
                table: "CourseNames",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseNames_Teacher_Id",
                table: "CourseNames",
                column: "Teacher_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseNames_Teachers_Teacher_Id",
                table: "CourseNames",
                column: "Teacher_Id",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id");
        }
    }
}
