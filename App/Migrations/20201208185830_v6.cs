using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HerosCount",
                table: "GameAccounts",
                newName: "HeroesCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeroesCount",
                table: "GameAccounts",
                newName: "HerosCount");
        }
    }
}
