using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class updateRequestDocumenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFiles",
                table: "RequestDocuments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RequestDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DataFiles",
                table: "RequestDocuments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RequestDocuments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
