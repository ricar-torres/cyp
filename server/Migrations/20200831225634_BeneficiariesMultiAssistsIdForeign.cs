using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class BeneficiariesMultiAssistsIdForeign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "alianza_id",
                table: "beneficiaries",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MultiAssistId",
                table: "beneficiaries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_beneficiaries_MultiAssistId",
                table: "beneficiaries",
                column: "MultiAssistId");

            migrationBuilder.AddForeignKey(
                name: "beneficiaries_multi_assists_id_foreign",
                table: "beneficiaries",
                column: "MultiAssistId",
                principalTable: "multi_assists",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "beneficiaries_multi_assists_id_foreign",
                table: "beneficiaries");

            migrationBuilder.DropIndex(
                name: "IX_beneficiaries_MultiAssistId",
                table: "beneficiaries");

            migrationBuilder.DropColumn(
                name: "MultiAssistId",
                table: "beneficiaries");

            migrationBuilder.AlterColumn<int>(
                name: "alianza_id",
                table: "beneficiaries",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
