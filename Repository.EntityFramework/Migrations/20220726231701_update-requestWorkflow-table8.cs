using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updaterequestWorkflowtable8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhoResponse",
                table: "RequestWorkflows");

            migrationBuilder.AddColumn<long>(
                name: "WhoResponseId",
                table: "RequestWorkflows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_WhoResponseId",
                table: "RequestWorkflows",
                column: "WhoResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestWorkflows_Employees_WhoResponseId",
                table: "RequestWorkflows",
                column: "WhoResponseId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestWorkflows_Employees_WhoResponseId",
                table: "RequestWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_RequestWorkflows_WhoResponseId",
                table: "RequestWorkflows");

            migrationBuilder.DropColumn(
                name: "WhoResponseId",
                table: "RequestWorkflows");

            migrationBuilder.AddColumn<long>(
                name: "WhoResponse",
                table: "RequestWorkflows",
                type: "bigint",
                nullable: true);
        }
    }
}
