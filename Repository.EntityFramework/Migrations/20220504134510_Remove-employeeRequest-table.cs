using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class RemoveemployeeRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequests_BenefitRequests_BenefitRequestId",
                table: "EmployeeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequests_Employees_EmployeeId",
                table: "EmployeeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequests_RequestStatus_RequestStatusId",
                table: "EmployeeRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRequests",
                table: "EmployeeRequests");

            migrationBuilder.RenameTable(
                name: "EmployeeRequests",
                newName: "EmployeeRequest");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeRequests_RequestStatusId",
                table: "EmployeeRequest",
                newName: "IX_EmployeeRequest_RequestStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeRequests_BenefitRequestId",
                table: "EmployeeRequest",
                newName: "IX_EmployeeRequest_BenefitRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRequest",
                table: "EmployeeRequest",
                columns: new[] { "EmployeeId", "BenefitRequestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequest_BenefitRequests_BenefitRequestId",
                table: "EmployeeRequest",
                column: "BenefitRequestId",
                principalTable: "BenefitRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequest_Employees_EmployeeId",
                table: "EmployeeRequest",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequest_RequestStatus_RequestStatusId",
                table: "EmployeeRequest",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequest_BenefitRequests_BenefitRequestId",
                table: "EmployeeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequest_Employees_EmployeeId",
                table: "EmployeeRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRequest_RequestStatus_RequestStatusId",
                table: "EmployeeRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRequest",
                table: "EmployeeRequest");

            migrationBuilder.RenameTable(
                name: "EmployeeRequest",
                newName: "EmployeeRequests");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeRequest_RequestStatusId",
                table: "EmployeeRequests",
                newName: "IX_EmployeeRequests_RequestStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeRequest_BenefitRequestId",
                table: "EmployeeRequests",
                newName: "IX_EmployeeRequests_BenefitRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRequests",
                table: "EmployeeRequests",
                columns: new[] { "EmployeeId", "BenefitRequestId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequests_BenefitRequests_BenefitRequestId",
                table: "EmployeeRequests",
                column: "BenefitRequestId",
                principalTable: "BenefitRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequests_Employees_EmployeeId",
                table: "EmployeeRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRequests_RequestStatus_RequestStatusId",
                table: "EmployeeRequests",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
