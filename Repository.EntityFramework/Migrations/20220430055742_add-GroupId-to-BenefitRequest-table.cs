using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addGroupIdtoBenefitRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "BenefitRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BenefitRequests_GroupId",
                table: "BenefitRequests",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitRequests_Groups_GroupId",
                table: "BenefitRequests",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitRequests_Groups_GroupId",
                table: "BenefitRequests");

            migrationBuilder.DropIndex(
                name: "IX_BenefitRequests_GroupId",
                table: "BenefitRequests");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "BenefitRequests");
        }
    }
}
