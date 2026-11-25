using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class statusss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "CourseNames",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseNames");
        }
    }
}
