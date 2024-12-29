using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentica.Service.Identity.Persistence
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_ACTIVITIES",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    sequence_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    activity_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_ACTIVITIES", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ACTIVITIESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_CLIENT_APPLICATIONS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    client_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    deleted_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    modified_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    client_secret = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    callback_uri = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true, defaultValueSql: "NEWID()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_CLIENT_APPLICATIONS", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_CLIENT_APPLICATIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_ROLES",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    deleted_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    modified_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    normalized_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_ROLES", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ROLESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_SESSIONS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    session_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    start_date_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    deleted_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime2", maxLength: 36, nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_SESSIONS", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_SESSIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_LOGIN",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_LOGIN", x => new { x.LoginProvider, x.ProviderKey });
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_LOGINHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    challenge_id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    challenge = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 12, 28, 6, 20, 3, 89, DateTimeKind.Utc).AddTicks(2083)),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGEHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    credential_id = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    public_key = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    user_handle = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    cred_type = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "public-key"),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    authenticator_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIALHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_TOKENS",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_TOKENS", x => new { x.UserId, x.LoginProvider, x.Name });
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_TOKENSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USERS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    address_city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address_country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address_name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    address_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    address_postcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    address_state = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address_street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    deleted_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    modified_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    normalized_username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    security_stamp = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    multi_factor_enabled = table.Column<bool>(type: "bit", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "bit", nullable: false),
                    access_failed_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USERS", x => x.id);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USERSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_ROLE_CLAIMS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    role_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    claim_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_ROLE_CLAIMS", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_IDENTITY_ROLE_CLAIMS_SYSTEM_IDENTITY_ROLES_role_id",
                        column: x => x.role_id,
                        principalTable: "SYSTEM_IDENTITY_ROLES",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ROLE_CLAIMSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_CLAIMS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    created_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    deleted_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    deleted_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    modified_by = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    claim_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_CLAIMS", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_IDENTITY_USER_CLAIMS_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_CLAIMSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    email_enabled = table.Column<bool>(type: "bit", nullable: false),
                    authenticator_enabled = table.Column<bool>(type: "bit", nullable: false),
                    passkeys_enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_USER_MFA_SETTINGS", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_IDENTITY_USER_MFA_SETTINGS_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_MFA_SETTINGSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    application_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS_SYSTEM_IDENTITY_CLIENT_APPLICATIONS_application_id",
                        column: x => x.application_id,
                        principalTable: "SYSTEM_IDENTITY_CLIENT_APPLICATIONS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    passkey_challenge_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE_SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE_passkey_challenge_id",
                        column: x => x.passkey_challenge_id,
                        principalTable: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGEHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    passkey_credential_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL", x => x.id);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL_SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL_passkey_credential_id",
                        column: x => x.passkey_credential_id,
                        principalTable: "SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIALHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SYSTEM_LINK_IDENTITY_USER_ROLES",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    role_id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_LINK_IDENTITY_USER_ROLES", x => new { x.role_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_ROLES_SYSTEM_IDENTITY_ROLES_role_id",
                        column: x => x.role_id,
                        principalTable: "SYSTEM_IDENTITY_ROLES",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYSTEM_LINK_IDENTITY_USER_ROLES_SYSTEM_IDENTITY_USERS_user_id",
                        column: x => x.user_id,
                        principalTable: "SYSTEM_IDENTITY_USERS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_ROLESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_ROLE_CLAIMS_role_id",
                table: "SYSTEM_IDENTITY_ROLE_CLAIMS",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "SYSTEM_IDENTITY_ROLES",
                column: "normalized_name",
                unique: true,
                filter: "[normalized_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_USER_CLAIMS_user_id",
                table: "SYSTEM_IDENTITY_USER_CLAIMS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_USER_MFA_SETTINGS_user_id",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "SYSTEM_IDENTITY_USERS",
                column: "normalized_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "SYSTEM_IDENTITY_USERS",
                column: "normalized_username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS_application_id",
                table: "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS_user_id",
                table: "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE_passkey_challenge_id",
                table: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE",
                column: "passkey_challenge_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE_user_id",
                table: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL_passkey_credential_id",
                table: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL",
                column: "passkey_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL_user_id",
                table: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_LINK_IDENTITY_USER_ROLES_user_id",
                table: "SYSTEM_LINK_IDENTITY_USER_ROLES",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_ACTIVITIES")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ACTIVITIESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_ROLE_CLAIMS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ROLE_CLAIMSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_SESSIONS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_SESSIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_CLAIMS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_CLAIMSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_LOGIN")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_LOGINHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_MFA_SETTINGS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_MFA_SETTINGSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_TOKENS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_TOKENSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_CLIENT_APPLICATIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGE")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CHALLENGEHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIAL")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_PASSKEY_CREDENTIALHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_LINK_IDENTITY_USER_ROLES")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_LINK_IDENTITY_USER_ROLESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_CLIENT_APPLICATIONS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_CLIENT_APPLICATIONSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGEHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIAL")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USER_PASSKEY_CREDENTIALHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_ROLES")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_ROLESHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_USERS")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SYSTEM_IDENTITY_USERSHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
