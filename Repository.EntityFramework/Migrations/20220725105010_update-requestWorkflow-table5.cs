using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updaterequestWorkflowtable5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestWorkflows",
                table: "RequestWorkflows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestWorkflows",
                table: "RequestWorkflows",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_BenefitRequestId",
                table: "RequestWorkflows",
                column: "BenefitRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestWorkflows",
                table: "RequestWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_RequestWorkflows_BenefitRequestId",
                table: "RequestWorkflows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestWorkflows",
                table: "RequestWorkflows",
                columns: new[] { "BenefitRequestId", "EmployeeId" });
        }
    }
}
