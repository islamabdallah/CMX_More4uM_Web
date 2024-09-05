using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addarabiccoloumnstobenefittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArabicDescription",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicTitle",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicDescription",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "ArabicTitle",
                table: "Benefits");
        }
    }
}
