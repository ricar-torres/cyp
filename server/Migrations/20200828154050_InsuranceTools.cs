using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class InsuranceTools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "beneficiary",
                table: "covers",
                nullable: false,
                defaultValueSql: "('0')");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "covers",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "insurance_addOns",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    health_plan_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(type: "VARCHAR(80)", maxLength: 255, nullable: false),
                    individual_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    coverage_single_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    coverage_couple_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    coverage_family_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    minimum_EE = table.Column<int>(nullable: false),
                    type_calculate = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_addOns", x => x.id);
                    table.ForeignKey(
                        name: "insurance_addOns_health_plan_id_foreign",
                        column: x => x.health_plan_id,
                        principalTable: "health_plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "insurance_benefit_type",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_benefit_type_id = table.Column<int>(nullable: false),
                    benefit_type = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    row_order = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_benefit_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "insurance_rate",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cover_id = table.Column<int>(nullable: false),
                    age = table.Column<int>(nullable: false),
                    rate_effective_date = table.Column<DateTime>(type: "date", nullable: false),
                    rate_expiration_date = table.Column<DateTime>(type: "date", nullable: false),
                    individual_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    individual_tobacco_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    policy_year = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    CoversId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_rate", x => x.id);
                    table.ForeignKey(
                        name: "FK_insurance_rate_covers_CoversId",
                        column: x => x.CoversId,
                        principalTable: "covers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "insurance_addOns_rate_age",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insurance_addOns_id = table.Column<int>(nullable: false),
                    age = table.Column<int>(nullable: false),
                    rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_addOns_rate_age", x => x.id);
                    table.ForeignKey(
                        name: "rates_by_age_insurance_addOns_id_foreign",
                        column: x => x.insurance_addOns_id,
                        principalTable: "insurance_addOns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsurancePlanAddOns",
                columns: table => new
                {
                    CoverId = table.Column<int>(nullable: false),
                    InsuranceAddOnsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlanAddOns", x => new { x.CoverId, x.InsuranceAddOnsId });
                    table.ForeignKey(
                        name: "FK_InsurancePlanAddOns_covers_CoverId",
                        column: x => x.CoverId,
                        principalTable: "covers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsurancePlanAddOns_insurance_addOns_InsuranceAddOnsId",
                        column: x => x.InsuranceAddOnsId,
                        principalTable: "insurance_addOns",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insurance_plan_benefit",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cover_id = table.Column<int>(nullable: false),
                    insurance_benefit_type_id = table.Column<int>(nullable: false),
                    value = table.Column<string>(type: "VARCHAR(max)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    CoversId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_plan_benefit", x => x.id);
                    table.ForeignKey(
                        name: "FK_insurance_plan_benefit_covers_CoversId",
                        column: x => x.CoversId,
                        principalTable: "covers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_insurance_plan_benefit_insurance_benefit_type_insurance_benefit_type_id",
                        column: x => x.insurance_benefit_type_id,
                        principalTable: "insurance_benefit_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_insurance_addOns_health_plan_id",
                table: "insurance_addOns",
                column: "health_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_addOns_rate_age_insurance_addOns_id",
                table: "insurance_addOns_rate_age",
                column: "insurance_addOns_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_plan_benefit_CoversId",
                table: "insurance_plan_benefit",
                column: "CoversId");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_plan_benefit_insurance_benefit_type_id",
                table: "insurance_plan_benefit",
                column: "insurance_benefit_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_rate_CoversId",
                table: "insurance_rate",
                column: "CoversId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlanAddOns_InsuranceAddOnsId",
                table: "InsurancePlanAddOns",
                column: "InsuranceAddOnsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insurance_addOns_rate_age");

            migrationBuilder.DropTable(
                name: "insurance_plan_benefit");

            migrationBuilder.DropTable(
                name: "insurance_rate");

            migrationBuilder.DropTable(
                name: "InsurancePlanAddOns");

            migrationBuilder.DropTable(
                name: "insurance_benefit_type");

            migrationBuilder.DropTable(
                name: "insurance_addOns");

            migrationBuilder.DropColumn(
                name: "beneficiary",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "type",
                table: "covers");
        }
    }
}
