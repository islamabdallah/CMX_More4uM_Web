using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updategrouptable6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BenefitId",
                table: "Groups",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BenefitId",
                table: "Groups",
                column: "BenefitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Benefits_BenefitId",
                table: "Groups",
                column: "BenefitId",
                principalTable: "Benefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Benefits_BenefitId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_BenefitId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "BenefitId",
                table: "Groups");
        }
    }
}
