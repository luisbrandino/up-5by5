using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPBank.Accounts.Migrations
{
    public partial class CreateAccountCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountCustomer",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerCpf = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCustomer", x => new { x.AccountNumber, x.CustomerCpf });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCustomer");
        }
    }
}
