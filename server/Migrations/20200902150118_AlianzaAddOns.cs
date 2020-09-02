using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AlianzaAddOns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alianza_addons",
                columns: table => new
                {
                    alianza_id = table.Column<int>(nullable: false),
                    insurance_addon_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alianza_addons", x => new { x.alianza_id, x.insurance_addon_id });
                    table.ForeignKey(
                        name: "alianza_addons_alianza_id_foreign",
                        column: x => x.alianza_id,
                        principalTable: "alianzas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "alianza_addons_insurance_addons_id_foreign",
                        column: x => x.insurance_addon_id,
                        principalTable: "insurance_addOns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alianza_addons_insurance_addon_id",
                table: "alianza_addons",
                column: "insurance_addon_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alianza_addons");
        }
    }
}
