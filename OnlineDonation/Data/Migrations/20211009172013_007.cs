using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDonation.Data.Migrations
{
    public partial class _007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonationId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonationId1",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DonationId1",
                table: "Transactions",
                column: "DonationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Donations_DonationId1",
                table: "Transactions",
                column: "DonationId1",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Donations_DonationId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DonationId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DonationId1",
                table: "Transactions");
        }
    }
}
