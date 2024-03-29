using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_artworks_ArtworkId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_Payment_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_ArtworkId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Payment_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_PaymentId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ArtworkId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "orders");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Artwork_Id",
                table: "orders",
                column: "Artwork_Id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Payment_Id",
                table: "orders",
                column: "Payment_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_User_Id",
                table: "orders",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_artworks_Artwork_Id",
                table: "orders",
                column: "Artwork_Id",
                principalTable: "artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_payments_Payment_Id",
                table: "orders",
                column: "Payment_Id",
                principalTable: "payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_User_Id",
                table: "orders",
                column: "User_Id",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_artworks_Artwork_Id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_payments_Payment_Id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_User_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Artwork_Id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_Payment_Id",
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

            migrationBuilder.AddColumn<long>(
                name: "ArtworkId",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_ArtworkId",
                table: "orders",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Payment_Id",
                table: "orders",
                column: "Payment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PaymentId",
                table: "orders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_artworks_ArtworkId",
                table: "orders",
                column: "ArtworkId",
                principalTable: "artworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_payments_PaymentId",
                table: "orders",
                column: "PaymentId",
                principalTable: "payments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_Payment_Id",
                table: "orders",
                column: "Payment_Id",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
