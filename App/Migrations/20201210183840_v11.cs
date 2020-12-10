using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "GemStonesCount",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "GemsCount",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "RankStartsCount",
                table: "GameAccounts");

            migrationBuilder.RenameColumn(
                name: "RubiesCount",
                table: "GameAccounts",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "GameAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameAccounts",
                newName: "RubiesCount");

            migrationBuilder.AlterColumn<string>(
                name: "LoginName",
                table: "GameAccounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RubiesCount",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GemStonesCount",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GemsCount",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RankStartsCount",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameAccounts",
                table: "GameAccounts",
                column: "LoginName");
        }
    }
}
