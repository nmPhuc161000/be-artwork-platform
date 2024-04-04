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
                name: "FK_orderdetailrequests_requestorders_Artwork_Id",
                table: "orderdetailrequests");

            migrationBuilder.RenameColumn(
                name: "Artwork_Id",
                table: "orderdetailrequests",
                newName: "Request_Id");

            migrationBuilder.RenameIndex(
                name: "IX_orderdetailrequests_Artwork_Id",
                table: "orderdetailrequests",
                newName: "IX_orderdetailrequests_Request_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderdetailrequests_requestorders_Request_Id",
                table: "orderdetailrequests",
                column: "Request_Id",
                principalTable: "requestorders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderdetailrequests_requestorders_Request_Id",
                table: "orderdetailrequests");

            migrationBuilder.RenameColumn(
                name: "Request_Id",
                table: "orderdetailrequests",
                newName: "Artwork_Id");

            migrationBuilder.RenameIndex(
                name: "IX_orderdetailrequests_Request_Id",
                table: "orderdetailrequests",
                newName: "IX_orderdetailrequests_Artwork_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderdetailrequests_requestorders_Artwork_Id",
                table: "orderdetailrequests",
                column: "Artwork_Id",
                principalTable: "requestorders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
