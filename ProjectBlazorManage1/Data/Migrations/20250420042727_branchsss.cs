using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class branchsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers");

            migrationBuilder.AlterColumn<int>(
                name: "JobPosition_Id",
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

            migrationBuilder.AddColumn<int>(
                name: "Branch_Id",
                table: "ClassStudieds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudieds_Branch_Id",
                table: "ClassStudieds",
                column: "Branch_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudieds_Branches_Branch_Id",
                table: "ClassStudieds",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Branch_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers",
                column: "Branch_Id",
                principalTable: "Branches",
                principalColumn: "Branch_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers",
                column: "JobPosition_Id",
                principalTable: "JobPositions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudieds_Branches_Branch_Id",
                table: "ClassStudieds");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Branches_Branch_Id",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_ClassStudieds_Branch_Id",
                table: "ClassStudieds");

            migrationBuilder.DropColumn(
                name: "Branch_Id",
                table: "ClassStudieds");

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
                name: "FK_Teachers_JobPositions_JobPosition_Id",
                table: "Teachers",
                column: "JobPosition_Id",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
