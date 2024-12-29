using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentica.Service.Identity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class userMFA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 23, 28, 1, 77, DateTimeKind.Utc).AddTicks(9555),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 12, 28, 6, 20, 3, 89, DateTimeKind.Utc).AddTicks(2083));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on_utc",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "modified_by",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "modified_on_utc",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS");

            migrationBuilder.DropColumn(
                name: "created_on_utc",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS");

            migrationBuilder.DropColumn(
                name: "modified_by",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS");

            migrationBuilder.DropColumn(
                name: "modified_on_utc",
                table: "SYSTEM_IDENTITY_USER_MFA_SETTINGS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 28, 6, 20, 3, 89, DateTimeKind.Utc).AddTicks(2083),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 12, 28, 23, 28, 1, 77, DateTimeKind.Utc).AddTicks(9555));
        }
    }
}
