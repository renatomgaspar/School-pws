using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_pws.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationDetailsTemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_AspNetUsers_UserId",
                table: "ApplicationDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Applications_ApplicationId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ApplicationId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDetails_UserId",
                table: "ApplicationDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ApplicationDetails");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "ApplicationDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationDetailsTemp",
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
                    table.PrimaryKey("PK_ApplicationDetailsTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationDetailsTemp_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationDetailsTemp_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_ApplicationId",
                table: "ApplicationDetails",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetailsTemp_SubjectId",
                table: "ApplicationDetailsTemp",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetailsTemp_UserId",
                table: "ApplicationDetailsTemp",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.DropTable(
                name: "ApplicationDetailsTemp");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationDetails_ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ApplicationDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ApplicationId",
                table: "Subjects",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationDetails_UserId",
                table: "ApplicationDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_AspNetUsers_UserId",
                table: "ApplicationDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Applications_ApplicationId",
                table: "Subjects",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
