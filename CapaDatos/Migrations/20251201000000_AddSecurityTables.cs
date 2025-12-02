using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CapaDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNT_LOCKOUTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FailedAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAttempt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNT_LOCKOUTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LOGIN_ATTEMPTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AttemptTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOGIN_ATTEMPTS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACCOUNT_LOCKOUTS_Email",
                table: "ACCOUNT_LOCKOUTS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOGIN_ATTEMPTS_Email_AttemptTime",
                table: "LOGIN_ATTEMPTS",
                columns: new[] { "Email", "AttemptTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCOUNT_LOCKOUTS");

            migrationBuilder.DropTable(
                name: "LOGIN_ATTEMPTS");
        }
    }
}
