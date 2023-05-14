using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBackEnd.Migrations
{
    public partial class DelayTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WorkingHourService_WorkingHourServiceId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "WorkingHourServiceId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Delays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delays_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "58979863-fdb1-4af6-9985-579e2780d166");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8e1bc287-7a5b-4b58-a2fa-fd7c05a84b3d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d46972c0-0c98-41e8-b3e9-f6bf4dcd7e9f");

            migrationBuilder.CreateIndex(
                name: "IX_Delays_UserId",
                table: "Delays",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WorkingHourService_WorkingHourServiceId",
                table: "Reservations",
                column: "WorkingHourServiceId",
                principalTable: "WorkingHourService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WorkingHourService_WorkingHourServiceId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Delays");

            migrationBuilder.AlterColumn<int>(
                name: "WorkingHourServiceId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e1c37fc0-371e-4b8e-9077-b88193127018");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "813043a7-0917-43a9-addf-cd2538eee442");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b4462308-9550-4e82-b494-fc9fe2d37c4e");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WorkingHourService_WorkingHourServiceId",
                table: "Reservations",
                column: "WorkingHourServiceId",
                principalTable: "WorkingHourService",
                principalColumn: "Id");
        }
    }
}
