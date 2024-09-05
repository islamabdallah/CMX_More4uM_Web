using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateRequestWorkflowtable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RequestWorkflows",
                newName: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestWorkflows_RequestStatusId",
                table: "RequestWorkflows",
                column: "RequestStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestWorkflows_RequestStatus_RequestStatusId",
                table: "RequestWorkflows",
                column: "RequestStatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestWorkflows_RequestStatus_RequestStatusId",
                table: "RequestWorkflows");

            migrationBuilder.DropIndex(
                name: "IX_RequestWorkflows_RequestStatusId",
                table: "RequestWorkflows");

            migrationBuilder.RenameColumn(
                name: "RequestStatusId",
                table: "RequestWorkflows",
                newName: "Status");
        }
    }
}
