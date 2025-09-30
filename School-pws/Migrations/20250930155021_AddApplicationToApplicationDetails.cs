using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_pws.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationToApplicationDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "ApplicationDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "ApplicationDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationDetails_Applications_ApplicationId",
                table: "ApplicationDetails",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
