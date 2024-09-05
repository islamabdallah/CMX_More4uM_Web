using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateBenefitRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "RequestStatusId",
                table: "BenefitRequests",
                type: "int",
                nullable: false
               );

            migrationBuilder.CreateIndex(
                name: "IX_BenefitRequests_RequestStatusId",
                table: "BenefitRequests",
                column: "RequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitRequests_RequestStatus_RequestStatusId",
                table: "BenefitRequests",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitRequests_RequestStatus_RequestStatusId",
                table: "BenefitRequests");

            migrationBuilder.DropIndex(
                name: "IX_BenefitRequests_RequestStatusId",
                table: "BenefitRequests");

            migrationBuilder.DropColumn(
                name: "RequestStatusId",
                table: "BenefitRequests");

           
        }
    }
}
