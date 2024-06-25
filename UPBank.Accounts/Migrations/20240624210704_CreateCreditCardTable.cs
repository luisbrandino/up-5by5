using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPBank.Accounts.Migrations
{
    public partial class CreateCreditCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreditCardNumber",
                table: "Account",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SavingsAccount",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    Number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExtractionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Limit = table.Column<double>(type: "float", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.Number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "SavingsAccount",
                table: "Account");
        }
    }
}
