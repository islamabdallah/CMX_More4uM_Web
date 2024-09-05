using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addIOSVersionAdroidVersioncoloumnMobileVersionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "MobileVersions");

            migrationBuilder.AddColumn<string>(
                name: "AndroidVersion",
                table: "MobileVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IOSVersion",
                table: "MobileVersions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AndroidVersion",
                table: "MobileVersions");

            migrationBuilder.DropColumn(
                name: "IOSVersion",
                table: "MobileVersions");

            migrationBuilder.AddColumn<float>(
                name: "Version",
                table: "MobileVersions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
