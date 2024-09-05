using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateNotificationtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserNotifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UserNotifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BenefitRequestId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BenefitRequestId",
                table: "Notifications",
                column: "BenefitRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications",
                column: "BenefitRequestId",
                principalTable: "BenefitRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications");

           

            migrationBuilder.DropIndex(
                name: "IX_Notifications_BenefitRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "BenefitRequestId",
                table: "Notifications");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

  
        }
    }
}
