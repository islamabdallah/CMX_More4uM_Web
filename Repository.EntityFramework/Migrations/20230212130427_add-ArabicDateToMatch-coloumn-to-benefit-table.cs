using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addArabicDateToMatchcoloumntobenefittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Benefits_BenefitType_BenefitTypeId",
            //    table: "Benefits");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_BenefitType",
            //    table: "BenefitType");

            //migrationBuilder.RenameTable(
            //    name: "BenefitType",
            //    newName: "BenefitTypes");

            migrationBuilder.AddColumn<string>(
                name: "ArabicDateToMatch",
                table: "Benefits",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_BenefitTypes",
            //    table: "BenefitTypes",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Benefits_BenefitTypes_BenefitTypeId",
            //    table: "Benefits",
            //    column: "BenefitTypeId",
            //    principalTable: "BenefitTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Benefits_BenefitTypes_BenefitTypeId",
            //    table: "Benefits");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_BenefitTypes",
            //    table: "BenefitTypes");

            migrationBuilder.DropColumn(
                name: "ArabicDateToMatch",
                table: "Benefits");

            //migrationBuilder.RenameTable(
            //    name: "BenefitTypes",
            //    newName: "BenefitType");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_BenefitType",
            //    table: "BenefitType",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Benefits_BenefitType_BenefitTypeId",
            //    table: "Benefits",
            //    column: "BenefitTypeId",
            //    principalTable: "BenefitType",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
