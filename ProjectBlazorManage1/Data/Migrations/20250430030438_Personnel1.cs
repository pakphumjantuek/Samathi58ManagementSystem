using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class Personnel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personnel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Initial_Id = table.Column<int>(type: "int", nullable: false),
                    Personnel_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personnel_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personnel_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Personnel_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    jobPosition_ID = table.Column<int>(type: "int", nullable: false),
                    EmployeePosition_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personnel_Initials_Initial_Id",
                        column: x => x.Initial_Id,
                        principalTable: "Initials",
                        principalColumn: "Initial_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personnel_JobPositions_EmployeePosition_ID",
                        column: x => x.EmployeePosition_ID,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_EmployeePosition_ID",
                table: "Personnel",
                column: "EmployeePosition_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_Initial_Id",
                table: "Personnel",
                column: "Initial_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personnel");
        }
    }
}
