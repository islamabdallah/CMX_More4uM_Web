using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addBenefitMailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Benefits_BenefitType_BenefitTypeId",
            //    table: "Benefits");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_BenefitType",
            //    table: "BenefitType");

            migrationBuilder.DropColumn(
                name: "BenefitReturn",
                table: "Benefits");

            //migrationBuilder.RenameTable(
            //    name: "BenefitType",
            //    newName: "BenefitTypes");

            migrationBuilder.AddColumn<bool>(
                name: "HasMails",
                table: "Benefits",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "HasMails",
                table: "Benefits");

            //migrationBuilder.RenameTable(
            //    name: "BenefitTypes",
            //    newName: "BenefitType");

            migrationBuilder.AddColumn<int>(
                name: "BenefitReturn",
                table: "Benefits",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
