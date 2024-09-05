using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.EntityFramework.Migrations
{
    public partial class CreateCertaindate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CertainDate",
                table: "Benefits",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertainDate",
                table: "Benefits");
        }
    }
}
