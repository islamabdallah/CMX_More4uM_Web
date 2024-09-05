using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.EntityFramework.Migrations
{
    public partial class addarcoloumntomedialDetailstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkingHours",
                table: "MedicalDetails",
                newName: "WorkingHours_EN");

            migrationBuilder.AddColumn<string>(
                name: "Address_EN",
                table: "MedicalDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours_AR",
                table: "MedicalDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_EN",
                table: "MedicalDetails");

            migrationBuilder.DropColumn(
                name: "WorkingHours_AR",
                table: "MedicalDetails");

            migrationBuilder.RenameColumn(
                name: "WorkingHours_EN",
                table: "MedicalDetails",
                newName: "WorkingHours");
        }
    }
}
