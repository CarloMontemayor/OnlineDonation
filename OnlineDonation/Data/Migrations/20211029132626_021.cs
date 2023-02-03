using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineDonation.Data.Migrations
{
    public partial class _021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowLocation",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnsweredLocation",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowLocation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AnsweredLocation",
                table: "AspNetUsers");
        }
    }
}
