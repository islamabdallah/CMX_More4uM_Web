using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class removeMailDescriptioncoloumnBenefitMailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailDescription",
                table: "BenefitMails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MailDescription",
                table: "BenefitMails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
