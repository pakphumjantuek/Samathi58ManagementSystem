using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class LastStudentRunningNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStudentRunningNumber",
                table: "ClassStudieds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastStudentRunningNumber",
                table: "ClassStudieds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
