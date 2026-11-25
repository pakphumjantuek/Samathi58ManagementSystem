using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Dhamacs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
