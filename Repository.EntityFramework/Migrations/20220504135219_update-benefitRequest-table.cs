using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatebenefitRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "BenefitRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_BenefitRequests_EmployeeId",
                table: "BenefitRequests",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BenefitRequests_Employees_EmployeeId",
                table: "BenefitRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenefitRequests_Employees_EmployeeId",
                table: "BenefitRequests");

            migrationBuilder.DropIndex(
                name: "IX_BenefitRequests_EmployeeId",
                table: "BenefitRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BenefitRequests");
        }
    }
}
