using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updatebenefitbenefitworkflowtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedDateFrom",
                table: "RequestWorkflows");

            migrationBuilder.DropColumn(
                name: "ConfirmedDateTo",
                table: "RequestWorkflows");

            migrationBuilder.AddColumn<int>(
                name: "Times",
                table: "Benefits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Times",
                table: "Benefits");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDateFrom",
                table: "RequestWorkflows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDateTo",
                table: "RequestWorkflows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
