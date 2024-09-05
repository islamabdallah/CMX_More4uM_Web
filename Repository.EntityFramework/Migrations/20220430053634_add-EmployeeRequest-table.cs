using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class addEmployeeRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EmployeeRequests",
                columns: table => new
                {
                    BenefitRequestId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    RequestStatusId = table.Column<int>(type: "int", nullable: false),
                    joinDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRequests", x => new { x.EmployeeId, x.BenefitRequestId });
                    table.ForeignKey(
                        name: "FK_EmployeeRequests_BenefitRequests_BenefitRequestId",
                        column: x => x.BenefitRequestId,
                        principalTable: "BenefitRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRequests_RequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRequests_BenefitRequestId",
                table: "EmployeeRequests",
                column: "BenefitRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRequests_RequestStatusId",
                table: "EmployeeRequests",
                column: "RequestStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeRequests");

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
    }
}
