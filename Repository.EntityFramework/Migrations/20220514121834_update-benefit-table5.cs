using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatebenefittable5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Collar",
                table: "Benefits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequiredDocuments",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numberOfDays",
                table: "Benefits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collar",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "RequiredDocuments",
                table: "Benefits");

            migrationBuilder.DropColumn(
                name: "numberOfDays",
                table: "Benefits");
        }
    }
}
