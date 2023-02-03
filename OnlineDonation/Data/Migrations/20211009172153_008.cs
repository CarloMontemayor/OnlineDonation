using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDonation.Data.Migrations
{
    public partial class _008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Donations_DonationId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DonationId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DonationId1",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "DonationId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DonationId",
                table: "Transactions",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Donations_DonationId",
                table: "Transactions",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Donations_DonationId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DonationId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "DonationId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
