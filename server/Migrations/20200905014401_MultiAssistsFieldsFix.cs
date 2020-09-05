using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class MultiAssistsFieldsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status_id",
                table: "multi_assists",
                type: "VARCHAR(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "routing_num",
                table: "multi_assists",
                type: "VARCHAR(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref3",
                table: "multi_assists",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref2",
                table: "multi_assists",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref1",
                table: "multi_assists",
                type: "VARCHAR(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exp_date",
                table: "multi_assists",
                type: "VARCHAR(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "deb_recurring_type",
                table: "multi_assists",
                type: "VARCHAR(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bank_name",
                table: "multi_assists",
                type: "VARCHAR(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_type",
                table: "multi_assists",
                type: "VARCHAR(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_num",
                table: "multi_assists",
                type: "VARCHAR(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_holder_name",
                table: "multi_assists",
                type: "VARCHAR(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 60,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status_id",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "routing_num",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(9)",
                oldMaxLength: 9,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref3",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref2",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ref1",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "exp_date",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "deb_recurring_type",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bank_name",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_type",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_num",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "account_holder_name",
                table: "multi_assists",
                type: "VARCHAR",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(60)",
                oldMaxLength: 60,
                oldNullable: true);
        }
    }
}
