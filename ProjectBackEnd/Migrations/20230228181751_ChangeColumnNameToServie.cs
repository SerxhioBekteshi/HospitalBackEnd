using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBackEnd.Migrations
{
    public partial class ChangeColumnNameToServie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceStaff_AspNetUsers_UserId",
                table: "ServiceStaff");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ServiceStaff",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceStaff_UserId",
                table: "ServiceStaff",
                newName: "IX_ServiceStaff_StaffId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8b9cb11d-08ec-4d0e-8b19-b0720285f891");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0aeb9973-dac4-4168-a37b-5b8a5c5b8af4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "30b5162e-3d47-4d37-a1d3-3199180bd182");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceStaff_AspNetUsers_StaffId",
                table: "ServiceStaff",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceStaff_AspNetUsers_StaffId",
                table: "ServiceStaff");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "ServiceStaff",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceStaff_StaffId",
                table: "ServiceStaff",
                newName: "IX_ServiceStaff_UserId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b3d3954c-86ea-4482-8776-ad0dec2df720");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "549ec2e2-185d-4444-8b2d-71a559fc25ed");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "79afc5e4-fb53-4228-81ef-baf37e7bcee5");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceStaff_AspNetUsers_UserId",
                table: "ServiceStaff",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
