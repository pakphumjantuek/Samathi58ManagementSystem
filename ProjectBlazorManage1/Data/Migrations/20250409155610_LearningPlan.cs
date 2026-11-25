using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class LearningPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Initials_Initial_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers");

            migrationBuilder.AlterColumn<string>(
                name: "Special_Skill",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobPosition_Id",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Initial_Id",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Branch_Id",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Branch_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Initials_Initial_Id",
                table: "Teachers",
                column: "Initial_Id",
                principalTable: "Initials",
                principalColumn: "Initial_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers",
                column: "JobPosition_Id",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Initials_Initial_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers");

            migrationBuilder.AlterColumn<string>(
                name: "Special_Skill",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "JobPosition_Id",
                table: "Teachers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Initial_Id",
                table: "Teachers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Branch_Id",
                table: "Teachers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Branch_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Initials_Initial_Id",
                table: "Teachers",
                column: "Initial_Id",
                principalTable: "Initials",
                principalColumn: "Initial_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers",
                column: "JobPosition_Id",
                principalTable: "JobPositions",
                principalColumn: "Id");
        }
    }
}
