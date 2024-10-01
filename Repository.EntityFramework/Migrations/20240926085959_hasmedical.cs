using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.EntityFramework.Migrations
{
    public partial class hasmedical : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMedicalService",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasMore4uService",
                table: "Employees",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {         
            migrationBuilder.DropColumn(
                name: "HasMedicalService",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "HasMore4uService",
                table: "Employees");
        }
    }
}
