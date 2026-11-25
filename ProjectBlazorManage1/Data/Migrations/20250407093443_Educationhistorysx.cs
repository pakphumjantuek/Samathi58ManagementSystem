using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Educationhistorysx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationHistory_Teachers_TeacherId",
                table: "EducationHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationHistory",
                table: "EducationHistory");

            migrationBuilder.RenameTable(
                name: "EducationHistory",
                newName: "EducationHistories");

            migrationBuilder.RenameIndex(
                name: "IX_EducationHistory_TeacherId",
                table: "EducationHistories",
                newName: "IX_EducationHistories_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationHistories",
                table: "EducationHistories",
                column: "EducationHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationHistories_Teachers_TeacherId",
                table: "EducationHistories",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationHistories_Teachers_TeacherId",
                table: "EducationHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationHistories",
                table: "EducationHistories");

            migrationBuilder.RenameTable(
                name: "EducationHistories",
                newName: "EducationHistory");

            migrationBuilder.RenameIndex(
                name: "IX_EducationHistories_TeacherId",
                table: "EducationHistory",
                newName: "IX_EducationHistory_TeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationHistory",
                table: "EducationHistory",
                column: "EducationHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationHistory_Teachers_TeacherId",
                table: "EducationHistory",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
