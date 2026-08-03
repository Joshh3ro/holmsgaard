using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holmsgaard.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Seed 10 employees with @hgaps.dk emails
            // Password for all: "HGAPS2026" hashed with SHA256 + "HolmsgaardSalt2026"
            var hash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes("HGAPS2026HolmsgaardSalt2026")));

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FullName", "Email", "PasswordHash", "HourlyRate", "IsActive" },
                values: new object[,]
                {
                    { Guid.Parse("11111111-1111-1111-1111-111111111111"), "Mikkel Svensson",       "mikkel.svensson@hgaps.dk",      hash, 375m, true },
                    { Guid.Parse("22222222-2222-2222-2222-222222222222"), "Anna Pedersen",        "anna.pedersen@hgaps.dk",       hash, 350m, true },
                    { Guid.Parse("33333333-3333-3333-3333-333333333333"), "Lars Jensen",          "lars.jensen@hgaps.dk",         hash, 400m, true },
                    { Guid.Parse("44444444-4444-4444-4444-444444444444"), "Mette Christoffersen", "mette.christoffersen@hgaps.dk",hash, 325m, true },
                    { Guid.Parse("55555555-5555-5555-5555-555555555555"), "Thomas Hansen",        "thomas.hansen@hgaps.dk",       hash, 380m, true },
                    { Guid.Parse("66666666-6666-6666-6666-666666666666"), "Sofie Madsen",         "sofie.madsen@hgaps.dk",        hash, 345m, true },
                    { Guid.Parse("77777777-7777-7777-7777-777777777777"), "Rasmus Kristensen",     "rasmus.kristensen@hgaps.dk",   hash, 360m, true },
                    { Guid.Parse("88888888-8888-8888-8888-888888888888"), "Emma Nielsen",          "emma.nielsen@hgaps.dk",        hash, 335m, true },
                    { Guid.Parse("99999999-9999-9999-9999-999999999999"), "Martin Olsen",          "martin.olsen@hgaps.dk",        hash, 390m, true },
                    { Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Frederikke Andersen",   "frederikke.andersen@hgaps.dk", hash, 355m, true },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Employees");
        }
    }
}
