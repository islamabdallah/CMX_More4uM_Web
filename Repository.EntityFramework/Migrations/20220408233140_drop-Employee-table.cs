using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.EntityFramework.Migrations
{
    public partial class dropEmployeetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDependent_Employees_EmployeeNumber",
                table: "EmployeeDependent");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Nationalities_NationalityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employee",
                newName: "IX_Employee_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PositionId",
                table: "Employee",
                newName: "IX_Employee_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_NationalityId",
                table: "Employee",
                newName: "IX_Employee_NationalityId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employee",
                newName: "IX_Employee_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                table: "Employee",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_SupervisorId",
                table: "Employee",
                column: "SupervisorId",
                principalTable: "Employee",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Nationalities_NationalityId",
                table: "Employee",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Positions_PositionId",
                table: "Employee",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDependent_Employee_EmployeeNumber",
                table: "EmployeeDependent",
                column: "EmployeeNumber",
                principalTable: "Employee",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_SupervisorId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Nationalities_NationalityId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Positions_PositionId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDependent_Employee_EmployeeNumber",
                table: "EmployeeDependent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_SupervisorId",
                table: "Employees",
                newName: "IX_Employees_SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_PositionId",
                table: "Employees",
                newName: "IX_Employees_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_NationalityId",
                table: "Employees",
                newName: "IX_Employees_NationalityId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDependent_Employees_EmployeeNumber",
                table: "EmployeeDependent",
                column: "EmployeeNumber",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId",
                principalTable: "Employees",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Nationalities_NationalityId",
                table: "Employees",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
