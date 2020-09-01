using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class MultiAssistsVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "multi_assists_vehicle",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    multi_assist_id = table.Column<int>(nullable: false),
                    make = table.Column<string>(maxLength: 30, nullable: false),
                    model = table.Column<string>(maxLength: 30, nullable: false),
                    year = table.Column<int>(nullable: false),
                    vin = table.Column<string>(maxLength: 17, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_multi_assists_vehicle", x => x.id);
                    table.ForeignKey(
                        name: "multi_assists_vehicle_multi_assists_id_foreign",
                        column: x => x.multi_assist_id,
                        principalTable: "multi_assists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_multi_assists_vehicle_multi_assist_id",
                table: "multi_assists_vehicle",
                column: "multi_assist_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "multi_assists_vehicle");
        }
    }
}
