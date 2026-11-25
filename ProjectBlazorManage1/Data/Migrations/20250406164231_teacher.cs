using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlazorManage1.Migrations
{
    /// <inheritdoc />
    public partial class teacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Teacher_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Teacher_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teacher_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Initial_Id = table.Column<int>(type: "int", nullable: true),
                    Branch_Id = table.Column<int>(type: "int", nullable: true),
                    JobPosition_Id = table.Column<int>(type: "int", nullable: true),
                    Position_detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Special_Skill = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Teacher_Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Branches_Branch_Id",
                        column: x => x.Branch_Id,
                        principalTable: "Branches",
                        principalColumn: "Branch_Id");
                    table.ForeignKey(
                        name: "FK_Teachers_Initials_Initial_Id",
                        column: x => x.Initial_Id,
                        principalTable: "Initials",
                        principalColumn: "Initial_Id");
                    table.ForeignKey(
                        name: "FK_Teachers_JobPositions_JobPosition_Id",
                        column: x => x.JobPosition_Id,
                        principalTable: "JobPositions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Branch_Id",
                table: "Teachers",
                column: "Branch_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Initial_Id",
                table: "Teachers",
                column: "Initial_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_JobPosition_Id",
                table: "Teachers",
                column: "JobPosition_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
