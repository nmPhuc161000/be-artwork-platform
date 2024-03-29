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
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_User_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_User_Id",
                table: "orders");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Payment_Id",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Payment_Id",
                table: "orders",
                column: "Payment_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_Payment_Id",
                table: "orders",
                column: "Payment_Id",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_Payment_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Payment_Id",
                table: "orders");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Payment_Id",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_orders_User_Id",
                table: "orders",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_User_Id",
                table: "orders",
                column: "User_Id",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
