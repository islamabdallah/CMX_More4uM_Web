using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addCarbonCopycoloumntoBenefitMailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "BenefitMails",
                newName: "CarbonCopy");

            migrationBuilder.AlterColumn<string>(
                name: "SendTo",
                table: "BenefitMails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarbonCopy",
                table: "BenefitMails",
                newName: "Subject");

            migrationBuilder.AlterColumn<string>(
                name: "SendTo",
                table: "BenefitMails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
