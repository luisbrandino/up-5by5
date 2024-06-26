using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UPBank.Accounts.Migrations
{
    public partial class AddColumnIsMainCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMainCustomer",
                table: "AccountCustomer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainCustomer",
                table: "AccountCustomer");
        }
    }
}
