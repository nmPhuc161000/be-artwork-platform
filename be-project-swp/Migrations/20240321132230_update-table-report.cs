using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class updatetablereport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason_Delete",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason_Delete",
                table: "Reports");
        }
    }
}
