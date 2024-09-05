using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addBenefitIdtoBenefitMailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BenefitId",
                table: "BenefitMails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BenefitMails_BenefitId",
                table: "BenefitMails",
                column: "BenefitId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitMails_Benefits_BenefitId",
                table: "BenefitMails",
                column: "BenefitId",
                principalTable: "Benefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitMails_Benefits_BenefitId",
                table: "BenefitMails");

            migrationBuilder.DropIndex(
                name: "IX_BenefitMails_BenefitId",
                table: "BenefitMails");

            migrationBuilder.DropColumn(
                name: "BenefitId",
                table: "BenefitMails");
        }
    }
}
