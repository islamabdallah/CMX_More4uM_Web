using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addIOSLinkcoloumnMobileVersionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AndroidVersion",
                table: "MobileVersions",
                newName: "Version");

            migrationBuilder.AddColumn<string>(
                name: "IOSLink",
                table: "MobileVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IOSLink",
                table: "MobileVersions");

            migrationBuilder.RenameColumn(
                name: "Version",
                table: "MobileVersions",
                newName: "AndroidVersion");
        }
    }
}
