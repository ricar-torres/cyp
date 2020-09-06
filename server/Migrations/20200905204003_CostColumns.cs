using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class CostColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "individual_tobacco_rate",
                table: "insurance_rate");

            migrationBuilder.AlterColumn<decimal>(
                name: "individual_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)",
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "coverage_couple_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "coverage_family_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "coverage_single_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "alianza_addons",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coverage_couple_rate",
                table: "insurance_rate");

            migrationBuilder.DropColumn(
                name: "coverage_family_rate",
                table: "insurance_rate");

            migrationBuilder.DropColumn(
                name: "coverage_single_rate",
                table: "insurance_rate");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "alianza_addons");

            migrationBuilder.AlterColumn<decimal>(
                name: "individual_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)",
                oldDefaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "individual_tobacco_rate",
                table: "insurance_rate",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
