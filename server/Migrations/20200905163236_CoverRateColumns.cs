using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class CoverRateColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "coverage_couple_rate",
                table: "covers",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "coverage_family_rate",
                table: "covers",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "coverage_single_rate",
                table: "covers",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<decimal>(
                name: "individual_rate",
                table: "covers",
                type: "decimal(12,2)",
                nullable: false,
                defaultValueSql: "(0.00)");

            migrationBuilder.AddColumn<int>(
                name: "minimum_EE",
                table: "covers",
                nullable: false,
                defaultValueSql: "(0)");

            migrationBuilder.AddColumn<int>(
                name: "type_calculate",
                table: "covers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coverage_couple_rate",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "coverage_family_rate",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "coverage_single_rate",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "individual_rate",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "minimum_EE",
                table: "covers");

            migrationBuilder.DropColumn(
                name: "type_calculate",
                table: "covers");
        }
    }
}
