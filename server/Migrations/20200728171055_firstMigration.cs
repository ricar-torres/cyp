using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "activity_log",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        log_name = table.Column<string>(maxLength: 255, nullable: true),
            //        description = table.Column<string>(nullable: false),
            //        subject_id = table.Column<int>(nullable: true),
            //        subject_type = table.Column<string>(maxLength: 255, nullable: true),
            //        causer_id = table.Column<int>(nullable: true),
            //        causer_type = table.Column<string>(maxLength: 255, nullable: true),
            //        properties = table.Column<string>(nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_activity_log", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "addresses",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        type = table.Column<byte>(nullable: true),
            //        line_1 = table.Column<string>(maxLength: 255, nullable: true),
            //        line_2 = table.Column<string>(maxLength: 255, nullable: true),
            //        country = table.Column<string>(maxLength: 255, nullable: true),
            //        state = table.Column<string>(maxLength: 255, nullable: true),
            //        city = table.Column<string>(maxLength: 255, nullable: true),
            //        zipcode = table.Column<string>(maxLength: 255, nullable: true),
            //        zip_4 = table.Column<string>(maxLength: 255, nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_addresses", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "affiliation_periods",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        stat_date = table.Column<DateTime>(type: "date", nullable: false),
            //        end_date = table.Column<DateTime>(type: "date", nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_affiliation_periods", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "agencies",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_agencies", x => x.id);
            //    });

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Key = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    PrimaryColor = table.Column<string>(type: "VARCHAR(6)", nullable: true),
                    DelFlag = table.Column<bool>(nullable: false),
                    EmailApiKey = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    SenderEmail = table.Column<string>(type: "VARCHAR(254)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDt = table.Column<DateTime>(nullable: false),
                    UpdDt = table.Column<DateTime>(nullable: false),
                    FCreateUserId = table.Column<int>(nullable: false),
                    FUpdUserId = table.Column<int>(nullable: false),
                    UpdHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    UpdSysHostDt = table.Column<DateTime>(nullable: false),
                    UpdSysHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    UpdSysSqlUser = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    IPAddress = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    DelFlag = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "bona_fides",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        code = table.Column<string>(maxLength: 255, nullable: true),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        siglas = table.Column<string>(maxLength: 255, nullable: true),
            //        phone = table.Column<string>(maxLength: 255, nullable: true),
            //        email = table.Column<string>(maxLength: 255, nullable: true),
            //        benefits = table.Column<string>(maxLength: 255, nullable: true),
            //        disclaimer = table.Column<string>(maxLength: 255, nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_bona_fides", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "call_reasons",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_call_reasons", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "campaigns",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        origin = table.Column<int>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_campaigns", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "canceled_reasons",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_canceled_reasons", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "communication_methods",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_communication_methods", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "countries",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        code = table.Column<string>(maxLength: 255, nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_countries", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "csv_datas",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        csv_filename = table.Column<string>(maxLength: 255, nullable: false),
            //        csv_header = table.Column<bool>(nullable: false, defaultValueSql: "('0')"),
            //        csv_data = table.Column<string>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_csv_datas", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "document_categories",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_document_categories", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "files",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        size = table.Column<string>(maxLength: 255, nullable: false),
            //        path = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_files", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "health_plans",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        url = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_health_plans", x => x.id);
            //    });

            migrationBuilder.CreateTable(
                name: "LoginProvider",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    ProviderKey = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    DelFlag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    Icon = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Url = table.Column<string>(type: "VARCHAR(2083)", nullable: true),
                    Type = table.Column<string>(type: "VARCHAR(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "password_resets",
            //    columns: table => new
            //    {
            //        email = table.Column<string>(maxLength: 255, nullable: false),
            //        token = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "products",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_products", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "qualifying_events",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        requirements = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_qualifying_events", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "regions",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_regions", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "retirements",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        code = table.Column<string>(maxLength: 255, nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_retirements", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "roles",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_roles", x => x.id);
            //    });

            migrationBuilder.CreateTable(
                name: "OneTimePassword",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdDt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    FCreateUserId = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    FUpdUserId = table.Column<int>(nullable: false, defaultValueSql: "0"),
                    UpdHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    UpdSysHostDt = table.Column<DateTime>(nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    UpdSysHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValueSql: "HOST_NAME()"),
                    UpdSysSqlUser = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValueSql: "SUSER_SNAME()"),
                    IPAddress = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    DelFlag = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    ApplicationId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR(254)", nullable: true),
                    OTP = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    ValidDays = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimePassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneTimePassword_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateTable(
            //    name: "chapters",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        bona_fide_id = table.Column<int>(nullable: false),
            //        quota = table.Column<double>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_chapters", x => x.id);
            //        table.ForeignKey(
            //            name: "chapters_bona_fide_id_foreign",
            //            column: x => x.bona_fide_id,
            //            principalTable: "bona_fides",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "canceled_categories",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        canceled_reasons_id = table.Column<int>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_canceled_categories", x => x.id);
            //        table.ForeignKey(
            //            name: "canceled_categories_canceled_reasons_id_foreign",
            //            column: x => x.canceled_reasons_id,
            //            principalTable: "canceled_reasons",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "states",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        country_id = table.Column<int>(nullable: false),
            //        code = table.Column<string>(maxLength: 255, nullable: true),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_states", x => x.id);
            //        table.ForeignKey(
            //            name: "states_country_id_foreign",
            //            column: x => x.country_id,
            //            principalTable: "countries",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "document_types",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        document_category_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_document_types", x => x.id);
            //        table.ForeignKey(
            //            name: "document_types_document_category_id_foreign",
            //            column: x => x.document_category_id,
            //            principalTable: "document_categories",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "covers",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        health_plan_id = table.Column<int>(nullable: false),
            //        code = table.Column<string>(maxLength: 255, nullable: true),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        sob = table.Column<string>(maxLength: 255, nullable: true),
            //        alianza = table.Column<bool>(nullable: false, defaultValueSql: "('0')"),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        sob_img = table.Column<string>(maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_covers", x => x.id);
            //        table.ForeignKey(
            //            name: "covers_health_plan_id_foreign",
            //            column: x => x.health_plan_id,
            //            principalTable: "health_plans",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdDt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    FCreateUserId = table.Column<int>(nullable: false),
                    FUpdUserId = table.Column<int>(nullable: false),
                    UpdHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    UpdSysHostDt = table.Column<DateTime>(nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    UpdSysHostNm = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValueSql: "HOST_NAME()"),
                    UpdSysSqlUser = table.Column<string>(type: "VARCHAR(100)", nullable: true, defaultValueSql: "SUSER_SNAME()"),
                    IPAddress = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    DelFlag = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR(254)", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(254)", nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    FailRetryQty = table.Column<int>(nullable: false),
                    LoginProviderId = table.Column<int>(nullable: false),
                    Reference1 = table.Column<string>(type: "VARCHAR(30)", nullable: true),
                    Reference2 = table.Column<string>(type: "VARCHAR(30)", nullable: true),
                    Reference3 = table.Column<string>(type: "VARCHAR(30)", nullable: true),
                    Reference4 = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    IsChgPwd = table.Column<bool>(nullable: false),
                    ExpirationDt = table.Column<DateTime>(nullable: false),
                    LastLoginDt = table.Column<DateTime>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false),
                    UserType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                    table.CheckConstraint("CK_AppUser_UserType", "UserType IN('ADMIN','USER') ");
                    table.ForeignKey(
                        name: "FK_AppUser_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUser_LoginProvider_LoginProviderId",
                        column: x => x.LoginProviderId,
                        principalTable: "LoginProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    MenuItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenu", x => x.Id);
                    table.UniqueConstraint("AK_RoleMenu_RoleId_MenuItemId", x => new { x.RoleId, x.MenuItemId });
                    table.ForeignKey(
                        name: "FK_RoleMenu_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenu_AppRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "cities",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        country_id = table.Column<int>(nullable: false),
            //        region_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cities", x => x.id);
            //        table.ForeignKey(
            //            name: "cities_country_id_foreign",
            //            column: x => x.country_id,
            //            principalTable: "countries",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "cities_region_id_foreign",
            //            column: x => x.region_id,
            //            principalTable: "regions",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "users",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        role_id = table.Column<int>(nullable: false),
            //        code = table.Column<string>(maxLength: 255, nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        last_name = table.Column<string>(maxLength: 255, nullable: false),
            //        phone = table.Column<string>(maxLength: 255, nullable: true),
            //        email = table.Column<string>(maxLength: 255, nullable: false),
            //        password = table.Column<string>(maxLength: 255, nullable: false),
            //        remember_token = table.Column<string>(maxLength: 100, nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_users", x => x.id);
            //        table.ForeignKey(
            //            name: "users_role_id_foreign",
            //            column: x => x.role_id,
            //            principalTable: "roles",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "canceled_subcategories",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        canceled_categories_id = table.Column<int>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_canceled_subcategories", x => x.id);
            //        table.ForeignKey(
            //            name: "canceled_subcategories_canceled_categories_id_foreign",
            //            column: x => x.canceled_categories_id,
            //            principalTable: "canceled_categories",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "clients",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        initial = table.Column<string>(maxLength: 255, nullable: true),
            //        last_name_1 = table.Column<string>(maxLength: 255, nullable: false),
            //        last_name_2 = table.Column<string>(maxLength: 255, nullable: true),
            //        ssn = table.Column<string>(maxLength: 255, nullable: true),
            //        gender = table.Column<byte>(nullable: true),
            //        birth_date = table.Column<DateTime>(type: "date", nullable: true),
            //        email = table.Column<string>(maxLength: 255, nullable: true),
            //        phone_1 = table.Column<string>(maxLength: 255, nullable: false),
            //        phone_2 = table.Column<string>(maxLength: 255, nullable: true),
            //        marital_status = table.Column<byte>(nullable: true),
            //        agency_id = table.Column<int>(nullable: false),
            //        contribution = table.Column<double>(nullable: true),
            //        retirement_id = table.Column<int>(nullable: false),
            //        cover_id = table.Column<int>(nullable: false),
            //        contract_number = table.Column<string>(maxLength: 255, nullable: true),
            //        effective_date = table.Column<DateTime>(type: "date", nullable: true),
            //        medicare_a = table.Column<bool>(nullable: true),
            //        medicare_b = table.Column<bool>(nullable: true),
            //        campaign_id = table.Column<int>(nullable: false),
            //        principal = table.Column<bool>(nullable: true),
            //        status = table.Column<byte>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        pre_register = table.Column<bool>(nullable: true, defaultValueSql: "((0))")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_clients", x => x.id);
            //        table.ForeignKey(
            //            name: "clients_agency_id_foreign",
            //            column: x => x.agency_id,
            //            principalTable: "agencies",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "clients_campaign_id_foreign",
            //            column: x => x.campaign_id,
            //            principalTable: "campaigns",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "clients_cover_id_foreign",
            //            column: x => x.cover_id,
            //            principalTable: "covers",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "clients_retirement_id_foreign",
            //            column: x => x.retirement_id,
            //            principalTable: "retirements",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_AppRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermission",
                columns: table => new
                {
                    RoleMenuId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermission", x => new { x.RoleMenuId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_MenuPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPermission_RoleMenu_RoleMenuId",
                        column: x => x.RoleMenuId,
                        principalTable: "RoleMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "zipcodes",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        city_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_zipcodes", x => x.id);
            //        table.ForeignKey(
            //            name: "zipcodes_city_id_foreign",
            //            column: x => x.city_id,
            //            principalTable: "cities",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "chapter_client",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        chapter_id = table.Column<int>(nullable: false),
            //        client_id = table.Column<int>(nullable: false),
            //        registration_date = table.Column<DateTime>(type: "date", nullable: true),
            //        new_registration = table.Column<bool>(nullable: true),
            //        primary = table.Column<bool>(nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_chapter_client", x => x.id);
            //        table.ForeignKey(
            //            name: "chapter_client_chapter_id_foreign",
            //            column: x => x.chapter_id,
            //            principalTable: "chapters",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "chapter_client_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "client_communication_method",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        communication_method_id = table.Column<int>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_client_communication_method", x => x.id);
            //        table.ForeignKey(
            //            name: "client_communication_method_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "client_communication_method_communication_method_id_foreign",
            //            column: x => x.communication_method_id,
            //            principalTable: "communication_methods",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "client_document_type",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        document_type_id = table.Column<int>(nullable: false),
            //        url = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        sob_img_url = table.Column<string>(maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_client_document_type", x => x.id);
            //        table.ForeignKey(
            //            name: "client_document_type_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "client_document_type_document_type_id_foreign",
            //            column: x => x.document_type_id,
            //            principalTable: "document_types",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "client_product",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        product_id = table.Column<int>(nullable: false),
            //        status = table.Column<byte>(nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_client_product", x => x.id);
            //        table.ForeignKey(
            //            name: "client_product_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "client_product_product_id_foreign",
            //            column: x => x.product_id,
            //            principalTable: "products",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "dependents",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: true),
            //        initial = table.Column<string>(maxLength: 255, nullable: true),
            //        last_name_1 = table.Column<string>(maxLength: 255, nullable: true),
            //        last_name_2 = table.Column<string>(maxLength: 255, nullable: true),
            //        ssn = table.Column<string>(maxLength: 255, nullable: true),
            //        gender = table.Column<byte>(nullable: true),
            //        birth_date = table.Column<DateTime>(type: "date", nullable: true),
            //        email = table.Column<string>(maxLength: 255, nullable: true),
            //        phone_1 = table.Column<string>(maxLength: 255, nullable: true),
            //        phone_2 = table.Column<string>(maxLength: 255, nullable: true),
            //        relationship = table.Column<byte>(nullable: true),
            //        cover_id = table.Column<int>(nullable: false),
            //        contract_number = table.Column<string>(maxLength: 255, nullable: true),
            //        effective_date = table.Column<DateTime>(type: "date", nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        city_id = table.Column<int>(nullable: true),
            //        agency_id = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_dependents", x => x.id);
            //        table.ForeignKey(
            //            name: "dependents_agency_id_foreign",
            //            column: x => x.agency_id,
            //            principalTable: "agencies",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "dependents_city_id_foreign",
            //            column: x => x.city_id,
            //            principalTable: "cities",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "dependents_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "dependents_cover_id_foreign",
            //            column: x => x.cover_id,
            //            principalTable: "covers",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "tutors",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        last_name = table.Column<string>(maxLength: 255, nullable: false),
            //        phone = table.Column<string>(maxLength: 255, nullable: false),
            //        phi_file_url = table.Column<string>(maxLength: 255, nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_tutors", x => x.id);
            //        table.ForeignKey(
            //            name: "tutors_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "prospects",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        last_name1 = table.Column<string>(maxLength: 255, nullable: false),
            //        last_name2 = table.Column<string>(maxLength: 255, nullable: false),
            //        address_1 = table.Column<string>(maxLength: 255, nullable: false),
            //        address_2 = table.Column<string>(maxLength: 255, nullable: false),
            //        city_id = table.Column<int>(nullable: false),
            //        country = table.Column<string>(maxLength: 255, nullable: false),
            //        zipcode_id = table.Column<int>(nullable: false),
            //        zip4 = table.Column<string>(name: "zip+4", maxLength: 255, nullable: false),
            //        phone = table.Column<string>(maxLength: 255, nullable: false),
            //        birth_date = table.Column<DateTime>(type: "date", nullable: false),
            //        ssn = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_prospects", x => x.id);
            //        table.ForeignKey(
            //            name: "prospects_city_id_foreign",
            //            column: x => x.city_id,
            //            principalTable: "cities",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "prospects_zipcode_id_foreign",
            //            column: x => x.zipcode_id,
            //            principalTable: "zipcodes",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "alianzas",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_product_id = table.Column<int>(nullable: false),
            //        qualifying_event_id = table.Column<int>(nullable: false),
            //        cover_id = table.Column<int>(nullable: false),
            //        start_date = table.Column<DateTime>(type: "date", nullable: true),
            //        elegible_date = table.Column<DateTime>(type: "date", nullable: false),
            //        end_date = table.Column<DateTime>(type: "date", nullable: true),
            //        end_reason = table.Column<string>(maxLength: 255, nullable: true),
            //        aff_type = table.Column<byte>(nullable: true),
            //        aff_status = table.Column<byte>(nullable: false),
            //        aff_flag = table.Column<string>(maxLength: 255, nullable: true),
            //        coordination = table.Column<bool>(nullable: true),
            //        life_insurance = table.Column<bool>(nullable: true),
            //        major_medical = table.Column<bool>(nullable: true),
            //        prima = table.Column<double>(nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        joint = table.Column<double>(nullable: true),
            //        cover_amount = table.Column<double>(nullable: true),
            //        life_insurance_amount = table.Column<double>(nullable: true),
            //        major_medical_amount = table.Column<double>(nullable: true),
            //        sub_total = table.Column<double>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_alianzas", x => x.id);
            //        table.ForeignKey(
            //            name: "alianzas_client_product_id_foreign",
            //            column: x => x.client_product_id,
            //            principalTable: "client_product",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "alianzas_cover_id_foreign",
            //            column: x => x.cover_id,
            //            principalTable: "covers",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "alianzas_qualifying_event_id_foreign",
            //            column: x => x.qualifying_event_id,
            //            principalTable: "qualifying_events",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "beneficiaries",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        alianza_id = table.Column<int>(nullable: false),
            //        name = table.Column<string>(maxLength: 255, nullable: false),
            //        gender = table.Column<string>(maxLength: 255, nullable: false),
            //        birth_date = table.Column<DateTime>(type: "date", nullable: false),
            //        relationship = table.Column<string>(maxLength: 255, nullable: false),
            //        percent = table.Column<string>(maxLength: 255, nullable: false),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        deleted_at = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_beneficiaries", x => x.id);
            //        table.ForeignKey(
            //            name: "beneficiaries_alianza_id_foreign",
            //            column: x => x.alianza_id,
            //            principalTable: "alianzas",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "client_user",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        client_id = table.Column<int>(nullable: false),
            //        user_id = table.Column<int>(nullable: false),
            //        confirmation_number = table.Column<string>(maxLength: 255, nullable: false),
            //        call_type = table.Column<byte>(nullable: false),
            //        comments = table.Column<string>(nullable: true),
            //        created_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
            //        alianza_id = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_client_user", x => x.id);
            //        table.ForeignKey(
            //            name: "client_user_alianza_id_foreign",
            //            column: x => x.alianza_id,
            //            principalTable: "alianzas",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "client_user_client_id_foreign",
            //            column: x => x.client_id,
            //            principalTable: "clients",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "client_user_user_id_foreign",
            //            column: x => x.user_id,
            //            principalTable: "users",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "activity_log_log_name_index",
            //    table: "activity_log",
            //    column: "log_name");

            //migrationBuilder.CreateIndex(
            //    name: "ix_cyprus_address_client_id_type",
            //    table: "addresses",
            //    columns: new[] { "line_1", "line_2", "city", "zipcode", "client_id", "type" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_alianzas_client_product_id",
            //    table: "alianzas",
            //    column: "client_product_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_alianzas_cover_id",
            //    table: "alianzas",
            //    column: "cover_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_alianzas_qualifying_event_id",
            //    table: "alianzas",
            //    column: "qualifying_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_AppRole_Name",
                table: "AppRole",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_LoginProviderId",
                table: "AppUser",
                column: "LoginProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_ApplicationId_UserName",
                table: "AppUser",
                columns: new[] { "ApplicationId", "UserName" },
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_beneficiaries_alianza_id",
            //    table: "beneficiaries",
            //    column: "alianza_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_canceled_categories_canceled_reasons_id",
            //    table: "canceled_categories",
            //    column: "canceled_reasons_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_canceled_subcategories_canceled_categories_id",
            //    table: "canceled_subcategories",
            //    column: "canceled_categories_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_chapter_client_chapter_id",
            //    table: "chapter_client",
            //    column: "chapter_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_chapter_client_client_id",
            //    table: "chapter_client",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_chapters_bona_fide_id",
            //    table: "chapters",
            //    column: "bona_fide_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cities_country_id",
            //    table: "cities",
            //    column: "country_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cities_region_id",
            //    table: "cities",
            //    column: "region_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_communication_method_client_id",
            //    table: "client_communication_method",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_communication_method_communication_method_id",
            //    table: "client_communication_method",
            //    column: "communication_method_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_document_type_client_id",
            //    table: "client_document_type",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_document_type_document_type_id",
            //    table: "client_document_type",
            //    column: "document_type_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_product_client_id",
            //    table: "client_product",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_product_product_id",
            //    table: "client_product",
            //    column: "product_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_user_client_id",
            //    table: "client_user",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_client_user_user_id",
            //    table: "client_user",
            //    column: "user_id");

            //migrationBuilder.CreateIndex(
            //    name: "client_user_user_id",
            //    table: "client_user",
            //    columns: new[] { "alianza_id", "user_id" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_clients_agency_id",
            //    table: "clients",
            //    column: "agency_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_clients_campaign_id",
            //    table: "clients",
            //    column: "campaign_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_clients_cover_id",
            //    table: "clients",
            //    column: "cover_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_clients_retirement_id",
            //    table: "clients",
            //    column: "retirement_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_covers_health_plan_id",
            //    table: "covers",
            //    column: "health_plan_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_dependents_agency_id",
            //    table: "dependents",
            //    column: "agency_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_dependents_city_id",
            //    table: "dependents",
            //    column: "city_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_dependents_client_id",
            //    table: "dependents",
            //    column: "client_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_dependents_cover_id",
            //    table: "dependents",
            //    column: "cover_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_document_types_document_category_id",
            //    table: "document_types",
            //    column: "document_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermission_PermissionId",
                table: "MenuPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePassword_ApplicationId_UserName",
                table: "OneTimePassword",
                columns: new[] { "ApplicationId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_OneTimePassword_ApplicationId_UserName_DelFlag",
                table: "OneTimePassword",
                columns: new[] { "ApplicationId", "UserName", "DelFlag" });

            //migrationBuilder.CreateIndex(
            //    name: "password_resets_email_index",
            //    table: "password_resets",
            //    column: "email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_prospects_city_id",
            //    table: "prospects",
            //    column: "city_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_prospects_zipcode_id",
            //    table: "prospects",
            //    column: "zipcode_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenu_MenuItemId",
                table: "RoleMenu",
                column: "MenuItemId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_states_country_id",
            //    table: "states",
            //    column: "country_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tutors_client_id",
            //    table: "tutors",
            //    column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "users_email_unique",
            //    table: "users",
            //    column: "email",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_users_role_id",
            //    table: "users",
            //    column: "role_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_zipcodes_city_id",
            //    table: "zipcodes",
            //    column: "city_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_log");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "affiliation_periods");

            migrationBuilder.DropTable(
                name: "beneficiaries");

            migrationBuilder.DropTable(
                name: "call_reasons");

            migrationBuilder.DropTable(
                name: "canceled_subcategories");

            migrationBuilder.DropTable(
                name: "chapter_client");

            migrationBuilder.DropTable(
                name: "client_communication_method");

            migrationBuilder.DropTable(
                name: "client_document_type");

            migrationBuilder.DropTable(
                name: "client_user");

            migrationBuilder.DropTable(
                name: "csv_datas");

            migrationBuilder.DropTable(
                name: "dependents");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "MenuPermission");

            migrationBuilder.DropTable(
                name: "OneTimePassword");

            migrationBuilder.DropTable(
                name: "password_resets");

            migrationBuilder.DropTable(
                name: "prospects");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "tutors");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "canceled_categories");

            migrationBuilder.DropTable(
                name: "chapters");

            migrationBuilder.DropTable(
                name: "communication_methods");

            migrationBuilder.DropTable(
                name: "document_types");

            migrationBuilder.DropTable(
                name: "alianzas");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "RoleMenu");

            migrationBuilder.DropTable(
                name: "zipcodes");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "canceled_reasons");

            migrationBuilder.DropTable(
                name: "bona_fides");

            migrationBuilder.DropTable(
                name: "document_categories");

            migrationBuilder.DropTable(
                name: "client_product");

            migrationBuilder.DropTable(
                name: "qualifying_events");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "LoginProvider");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "agencies");

            migrationBuilder.DropTable(
                name: "campaigns");

            migrationBuilder.DropTable(
                name: "covers");

            migrationBuilder.DropTable(
                name: "retirements");

            migrationBuilder.DropTable(
                name: "health_plans");
        }
    }
}
