using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "GameAccounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts",
                column: "LoginName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "GameAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts",
                column: "Id");
        }
    }
}
