using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_Ranks_RankId",
                table: "GameAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_GameAccounts_RankId",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ranks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "RankName",
                table: "GameAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_RankName",
                table: "GameAccounts",
                column: "RankName");

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_Ranks_RankName",
                table: "GameAccounts",
                column: "RankName",
                principalTable: "Ranks",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_Ranks_RankName",
                table: "GameAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_GameAccounts_RankName",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "RankName",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Ranks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ranks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_RankId",
                table: "GameAccounts",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_Ranks_RankId",
                table: "GameAccounts",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
