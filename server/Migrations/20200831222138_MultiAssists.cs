using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class MultiAssists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "multi_assists",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_product_id = table.Column<int>(nullable: false),
                    cover_id = table.Column<int>(nullable: false),
                    efective_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    eligible_waiting_period_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sent_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    cost = table.Column<double>(nullable: true),
                    ref1 = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: true),
                    ref2 = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: true),
                    ref3 = table.Column<string>(type: "VARCHAR", maxLength: 30, nullable: true),
                    status_id = table.Column<int>(nullable: false),
                    account_type = table.Column<string>(type: "VARCHAR", maxLength: 4, nullable: true),
                    bank_name = table.Column<string>(type: "VARCHAR", maxLength: 60, nullable: true),
                    account_holder_name = table.Column<string>(type: "VARCHAR", maxLength: 60, nullable: true),
                    routing_num = table.Column<string>(type: "VARCHAR", maxLength: 9, nullable: true),
                    account_num = table.Column<string>(type: "VARCHAR", maxLength: 12, nullable: true),
                    exp_date = table.Column<string>(type: "VARCHAR", maxLength: 4, nullable: true),
                    deb_day = table.Column<int>(nullable: true),
                    deb_recurring_type = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_multi_assists", x => x.id);
                    table.ForeignKey(
                        name: "multi_assists_client_product_id_foreign",
                        column: x => x.client_product_id,
                        principalTable: "client_product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "multi_assists_cover_id_foreign",
                        column: x => x.cover_id,
                        principalTable: "covers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_multi_assists_client_product_id",
                table: "multi_assists",
                column: "client_product_id");

            migrationBuilder.CreateIndex(
                name: "IX_multi_assists_cover_id",
                table: "multi_assists",
                column: "cover_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "multi_assists");
        }
    }
}
