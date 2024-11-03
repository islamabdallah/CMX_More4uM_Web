using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.EntityFramework.Migrations
{
    public partial class itemCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications");

            migrationBuilder.AlterColumn<long>(
                name: "BenefitRequestId",
                table: "Notifications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

         
            migrationBuilder.CreateTable(
                name: "MedicalRequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relatives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Relatives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CoverPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ArabicRelatives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicRelation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedBy = table.Column<long>(type: "bigint", nullable: false),
                    RequestedFor = table.Column<int>(type: "int", nullable: false),
                    MonthlyMedication = table.Column<bool>(type: "bit", nullable: false),
                    RequestMedicalEntity = table.Column<int>(type: "int", nullable: true),
                    MedicalPurpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseMedicalEntity = table.Column<int>(type: "int", nullable: true),
                    ResponseFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRequests_MedicalRequestTypes_MedicalRequestTypeId",
                        column: x => x.MedicalRequestTypeId,
                        principalTable: "MedicalRequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalAttachments_MedicalRequests_MedicalRequestId",
                        column: x => x.MedicalRequestId,
                        principalTable: "MedicalRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRequestLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRequestId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRequestLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalRequestLogs_MedicalRequests_MedicalRequestId",
                        column: x => x.MedicalRequestId,
                        principalTable: "MedicalRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRequestId = table.Column<long>(type: "bigint", nullable: false),
                    MedicalItemId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalResponses_MedicalRequests_MedicalRequestId",
                        column: x => x.MedicalRequestId,
                        principalTable: "MedicalRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAttachments_MedicalRequestId",
                table: "MedicalAttachments",
                column: "MedicalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRequestLogs_MedicalRequestId",
                table: "MedicalRequestLogs",
                column: "MedicalRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRequests_MedicalRequestTypeId",
                table: "MedicalRequests",
                column: "MedicalRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalResponses_MedicalRequestId",
                table: "MedicalResponses",
                column: "MedicalRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications",
                column: "BenefitRequestId",
                principalTable: "BenefitRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "MedicalAttachments");

            migrationBuilder.DropTable(
                name: "MedicalItems");

            migrationBuilder.DropTable(
                name: "MedicalRequestLogs");

            migrationBuilder.DropTable(
                name: "MedicalResponses");

            migrationBuilder.DropTable(
                name: "Relatives");

            migrationBuilder.DropTable(
                name: "MedicalRequests");

            migrationBuilder.DropTable(
                name: "MedicalRequestTypes");

            migrationBuilder.DropColumn(
                name: "MedicalRequestId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "HasMedicalService",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "HasMore4uService",
                table: "Employees");

            migrationBuilder.AlterColumn<long>(
                name: "BenefitRequestId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_BenefitRequests_BenefitRequestId",
                table: "Notifications",
                column: "BenefitRequestId",
                principalTable: "BenefitRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
