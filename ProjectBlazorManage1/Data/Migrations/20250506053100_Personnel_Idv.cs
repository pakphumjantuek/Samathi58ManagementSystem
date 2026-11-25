using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Personnel_Idv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DhammaCourses_Teachers_TeacherId",
                table: "DhammaCourses");

            migrationBuilder.AlterColumn<string>(
                name: "Personnel_Email",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Personnel_Address",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "DhammaCourses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Personnel_Id",
                table: "DhammaCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DhammaCourses_Personnel_Id",
                table: "DhammaCourses",
                column: "Personnel_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DhammaCourses_Personnel_Personnel_Id",
                table: "DhammaCourses",
                column: "Personnel_Id",
                principalTable: "Personnel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DhammaCourses_Teachers_TeacherId",
                table: "DhammaCourses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DhammaCourses_Personnel_Personnel_Id",
                table: "DhammaCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_DhammaCourses_Teachers_TeacherId",
                table: "DhammaCourses");

            migrationBuilder.DropIndex(
                name: "IX_DhammaCourses_Personnel_Id",
                table: "DhammaCourses");

            migrationBuilder.DropColumn(
                name: "Personnel_Id",
                table: "DhammaCourses");

            migrationBuilder.AlterColumn<string>(
                name: "Personnel_Email",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Personnel_Address",
                table: "Personnel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "DhammaCourses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DhammaCourses_Teachers_TeacherId",
                table: "DhammaCourses",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Teacher_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
