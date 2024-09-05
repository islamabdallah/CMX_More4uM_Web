using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreForYou.Migrations
{
    public partial class addArabicRoleNametoAspNetRoletable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArabicRoleName",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicRoleName",
                table: "AspNetRoles");
        }
    }
}
