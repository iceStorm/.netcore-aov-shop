using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_Clients_ClientAccountId",
                table: "GameAccounts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.RenameColumn(
                name: "ClientAccountId",
                table: "GameAccounts",
                newName: "UserAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_GameAccounts_ClientAccountId",
                table: "GameAccounts",
                newName: "IX_GameAccounts_UserAccountId");

            migrationBuilder.AddColumn<int>(
                name: "TotalMoney",
                table: "AdminAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_AdminAccounts_UserAccountId",
                table: "GameAccounts",
                column: "UserAccountId",
                principalTable: "AdminAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_AdminAccounts_UserAccountId",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "AdminAccounts");

            migrationBuilder.RenameColumn(
                name: "UserAccountId",
                table: "GameAccounts",
                newName: "ClientAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_GameAccounts_UserAccountId",
                table: "GameAccounts",
                newName: "IX_GameAccounts_ClientAccountId");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalMoney = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_AdminAccounts_Id",
                        column: x => x.Id,
                        principalTable: "AdminAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_Clients_ClientAccountId",
                table: "GameAccounts",
                column: "ClientAccountId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
