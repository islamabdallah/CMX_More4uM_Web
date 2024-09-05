using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addCountrytomedicalDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MedicalDetails_Company_CompanyId",
            //    table: "MedicalDetails");

            //migrationBuilder.DropIndex(
            //    name: "IX_MedicalDetails_CompanyId",
            //    table: "MedicalDetails");

            //migrationBuilder.DropColumn(
            //    name: "CompanyId",
            //    table: "MedicalDetails");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "MedicalDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "MedicalDetails");

            //migrationBuilder.AddColumn<int>(
            //    name: "CompanyId",
            //    table: "MedicalDetails",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MedicalDetails_CompanyId",
            //    table: "MedicalDetails",
            //    column: "CompanyId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MedicalDetails_Company_CompanyId",
            //    table: "MedicalDetails",
            //    column: "CompanyId",
            //    principalTable: "Company",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
