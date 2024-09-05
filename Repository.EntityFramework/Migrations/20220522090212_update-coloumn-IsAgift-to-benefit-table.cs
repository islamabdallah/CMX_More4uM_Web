using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatecoloumnIsAgifttobenefittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiftTo",
                table: "Benefits");

            migrationBuilder.AddColumn<bool>(
                name: "IsAgift",
                table: "Benefits",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgift",
                table: "Benefits");

            migrationBuilder.AddColumn<int>(
                name: "GiftTo",
                table: "Benefits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
