using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class statusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalId",
                table: "students");

            migrationBuilder.AddColumn<string>(
                name: "Status_Medical",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Student_Id",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status_Medical",
                table: "students");

            migrationBuilder.DropColumn(
                name: "Student_Id",
                table: "students");

            migrationBuilder.AddColumn<string>(
                name: "PersonalId",
                table: "students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
