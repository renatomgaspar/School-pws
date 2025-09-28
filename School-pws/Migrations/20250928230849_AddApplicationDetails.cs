using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_pws.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Subjects_SubjectId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_SubjectId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationDetails_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ApplicationId",
                table: "Subjects",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_SubjectId",
                table: "ApplicationDetails",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_UserId",
                table: "ApplicationDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Applications_ApplicationId",
                table: "Subjects",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Applications_ApplicationId",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "ApplicationDetails");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ApplicationId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_SubjectId",
                table: "Applications",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Subjects_SubjectId",
                table: "Applications",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
