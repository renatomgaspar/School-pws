using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_pws.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGradeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_UserId1",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId1",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_UserId1",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Applications_UserId1",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Applications");

            migrationBuilder.AlterColumn<float>(
                name: "Grade",
                table: "ApplicationDetails",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId",
                table: "Subjects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId",
                table: "Subjects");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Subjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "ApplicationDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_UserId1",
                table: "Subjects",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId1",
                table: "Applications",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_UserId1",
                table: "Applications",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId",
                table: "Subjects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_UserId1",
                table: "Subjects",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
