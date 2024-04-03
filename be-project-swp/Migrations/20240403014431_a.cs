using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId_Receivier",
                table: "requestorders");

            migrationBuilder.DropColumn(
                name: "UserName_Sender",
                table: "requestorders");

            migrationBuilder.AddColumn<string>(
                name: "UserId_Sender",
                table: "requestorders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_requestorders_UserId_Sender",
                table: "requestorders",
                column: "UserId_Sender");

            migrationBuilder.AddForeignKey(
                name: "FK_requestorders_users_UserId_Sender",
                table: "requestorders",
                column: "UserId_Sender",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requestorders_users_UserId_Sender",
                table: "requestorders");

            migrationBuilder.DropIndex(
                name: "IX_requestorders_UserId_Sender",
                table: "requestorders");

            migrationBuilder.DropColumn(
                name: "UserId_Sender",
                table: "requestorders");

            migrationBuilder.AddColumn<string>(
                name: "UserId_Receivier",
                table: "requestorders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName_Sender",
                table: "requestorders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
