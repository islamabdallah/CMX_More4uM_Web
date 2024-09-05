using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updaterequestWorkflowtable9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestWorkflows_Employees_WhoResponseId",
                table: "RequestWorkflows");

            migrationBuilder.AlterColumn<long>(
                name: "WhoResponseId",
                table: "RequestWorkflows",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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

            migrationBuilder.AlterColumn<long>(
                name: "WhoResponseId",
                table: "RequestWorkflows",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestWorkflows_Employees_WhoResponseId",
                table: "RequestWorkflows",
                column: "WhoResponseId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
