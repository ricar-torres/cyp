using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class InsuranceTools2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_insurance_plan_benefit_covers_CoversId",
                table: "insurance_plan_benefit");

            migrationBuilder.DropForeignKey(
                name: "FK_insurance_rate_covers_CoversId",
                table: "insurance_rate");

            migrationBuilder.DropIndex(
                name: "IX_insurance_rate_CoversId",
                table: "insurance_rate");

            migrationBuilder.DropIndex(
                name: "IX_insurance_plan_benefit_CoversId",
                table: "insurance_plan_benefit");

            migrationBuilder.DropColumn(
                name: "CoversId",
                table: "insurance_rate");

            migrationBuilder.DropColumn(
                name: "CoversId",
                table: "insurance_plan_benefit");

            migrationBuilder.AddColumn<bool>(
                name: "Beneficiary",
                table: "insurance_addOns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_insurance_rate_cover_id",
                table: "insurance_rate",
                column: "cover_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_plan_benefit_cover_id",
                table: "insurance_plan_benefit",
                column: "cover_id");

            migrationBuilder.AddForeignKey(
                name: "cover_insurance_plan_benefit_foreign",
                table: "insurance_plan_benefit",
                column: "cover_id",
                principalTable: "covers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "cover_insurance_rate_foreign",
                table: "insurance_rate",
                column: "cover_id",
                principalTable: "covers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("Update dbo.covers set type='-65' where alianza = 1 and (name like '%PSM%' or name like '%First%' or name like '%FM %')");

            migrationBuilder.Sql("Update dbo.covers set type='+65' where alianza = 1 and (name like '%TSA%' or name like '%MMM%'  or name like '%Triple-S%')");


        }
       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "cover_insurance_plan_benefit_foreign",
                table: "insurance_plan_benefit");

            migrationBuilder.DropForeignKey(
                name: "cover_insurance_rate_foreign",
                table: "insurance_rate");

            migrationBuilder.DropIndex(
                name: "IX_insurance_rate_cover_id",
                table: "insurance_rate");

            migrationBuilder.DropIndex(
                name: "IX_insurance_plan_benefit_cover_id",
                table: "insurance_plan_benefit");

            migrationBuilder.DropColumn(
                name: "Beneficiary",
                table: "insurance_addOns");

            migrationBuilder.AddColumn<int>(
                name: "CoversId",
                table: "insurance_rate",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoversId",
                table: "insurance_plan_benefit",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_insurance_rate_CoversId",
                table: "insurance_rate",
                column: "CoversId");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_plan_benefit_CoversId",
                table: "insurance_plan_benefit",
                column: "CoversId");

            migrationBuilder.AddForeignKey(
                name: "FK_insurance_plan_benefit_covers_CoversId",
                table: "insurance_plan_benefit",
                column: "CoversId",
                principalTable: "covers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_insurance_rate_covers_CoversId",
                table: "insurance_rate",
                column: "CoversId",
                principalTable: "covers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
