using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentica.Service.Identity.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Outbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 6, 15, 17, 8, 27, 38, DateTimeKind.Utc).AddTicks(4159),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 1, 22, 12, 45, 5, 623, DateTimeKind.Utc).AddTicks(3936));

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_INBOX_STATE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceiveCount = table.Column<int>(type: "int", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Consumed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_INBOX_STATE", x => x.Id);
                    table.UniqueConstraint("AK_SYSTEM_IDENTITY_INBOX_STATE_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_OUTBOX_STATE",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Delivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_OUTBOX_STATE", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "SYSTEM_IDENTITY_OUTBOX_MESSAGES",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnqueueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYSTEM_IDENTITY_OUTBOX_MESSAGES", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_SYSTEM_IDENTITY_OUTBOX_MESSAGES_SYSTEM_IDENTITY_INBOX_STATE_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalTable: "SYSTEM_IDENTITY_INBOX_STATE",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_SYSTEM_IDENTITY_OUTBOX_MESSAGES_SYSTEM_IDENTITY_OUTBOX_STATE_OutboxId",
                        column: x => x.OutboxId,
                        principalTable: "SYSTEM_IDENTITY_OUTBOX_STATE",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_INBOX_STATE_Delivered",
                table: "SYSTEM_IDENTITY_INBOX_STATE",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_OUTBOX_MESSAGES_EnqueueTime",
                table: "SYSTEM_IDENTITY_OUTBOX_MESSAGES",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_OUTBOX_MESSAGES_ExpirationTime",
                table: "SYSTEM_IDENTITY_OUTBOX_MESSAGES",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_OUTBOX_MESSAGES_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "SYSTEM_IDENTITY_OUTBOX_MESSAGES",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true,
                filter: "[InboxMessageId] IS NOT NULL AND [InboxConsumerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_OUTBOX_MESSAGES_OutboxId_SequenceNumber",
                table: "SYSTEM_IDENTITY_OUTBOX_MESSAGES",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true,
                filter: "[OutboxId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SYSTEM_IDENTITY_OUTBOX_STATE_Created",
                table: "SYSTEM_IDENTITY_OUTBOX_STATE",
                column: "Created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_OUTBOX_MESSAGES");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_INBOX_STATE");

            migrationBuilder.DropTable(
                name: "SYSTEM_IDENTITY_OUTBOX_STATE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "SYSTEM_IDENTITY_USER_PASSKEY_CHALLENGE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 22, 12, 45, 5, 623, DateTimeKind.Utc).AddTicks(3936),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 6, 15, 17, 8, 27, 38, DateTimeKind.Utc).AddTicks(4159));
        }
    }
}
