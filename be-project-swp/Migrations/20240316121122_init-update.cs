using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class initupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpiration",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiration",
                table: "users",
                type: "datetime2",
                nullable: true);
        }
    }
}
