using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevJobs.Api.Migrations.EmailMessaging
{
    /// <inheritdoc />
    public partial class EmailMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailQueueItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueueItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTrackers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTrackers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedEmailQueueItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedEmailQueueItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueueItems");

            migrationBuilder.DropTable(
                name: "EmailTrackers");

            migrationBuilder.DropTable(
                name: "FailedEmailQueueItems");
        }
    }
}
