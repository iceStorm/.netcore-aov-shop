using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "GameAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "GameAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
