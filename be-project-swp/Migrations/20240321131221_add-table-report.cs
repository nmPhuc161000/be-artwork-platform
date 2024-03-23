using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class addtablereport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_users_User_Id",
                table: "Wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets");

            migrationBuilder.RenameTable(
                name: "Wallets",
                newName: "wallets");

            migrationBuilder.RenameIndex(
                name: "IX_Wallets_User_Id",
                table: "wallets",
                newName: "IX_wallets_User_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wallets",
                table: "wallets",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NickName_Reporter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NickName_Accused = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_wallets_users_User_Id",
                table: "wallets",
                column: "User_Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wallets_users_User_Id",
                table: "wallets");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wallets",
                table: "wallets");

            migrationBuilder.RenameTable(
                name: "wallets",
                newName: "Wallets");

            migrationBuilder.RenameIndex(
                name: "IX_wallets_User_Id",
                table: "Wallets",
                newName: "IX_Wallets_User_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wallets",
                table: "Wallets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_users_User_Id",
                table: "Wallets",
                column: "User_Id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
