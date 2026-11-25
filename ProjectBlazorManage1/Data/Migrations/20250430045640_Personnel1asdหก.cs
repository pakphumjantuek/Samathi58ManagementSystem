using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Personnel1asdหก : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_JobPositions_EmployeePosition_ID",
                table: "Personnel");

            migrationBuilder.DropIndex(
                name: "IX_Personnel_EmployeePosition_ID",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "EmployeePosition_ID",
                table: "Personnel");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_jobPosition_ID",
                table: "Personnel",
                column: "jobPosition_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_JobPositions_jobPosition_ID",
                table: "Personnel",
                column: "jobPosition_ID",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_JobPositions_jobPosition_ID",
                table: "Personnel");

            migrationBuilder.DropIndex(
                name: "IX_Personnel_jobPosition_ID",
                table: "Personnel");

            migrationBuilder.AddColumn<int>(
                name: "EmployeePosition_ID",
                table: "Personnel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_EmployeePosition_ID",
                table: "Personnel",
                column: "EmployeePosition_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_JobPositions_EmployeePosition_ID",
                table: "Personnel",
                column: "EmployeePosition_ID",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
