using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holmsgaard.ApiService.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260801120000_UpgradeSeededPasswordHashes")]
public sealed class UpgradeSeededPasswordHashes : Migration
{
    private static readonly (Guid Id, string Name, string Email, decimal HourlyRate)[] SeededEmployees =
    {
        (Guid.Parse("11111111-1111-1111-1111-111111111111"), "Mikkel Svensson", "mikkel.svensson@hgaps.dk", 375m),
        (Guid.Parse("22222222-2222-2222-2222-222222222222"), "Anna Pedersen", "anna.pedersen@hgaps.dk", 350m),
        (Guid.Parse("33333333-3333-3333-3333-333333333333"), "Lars Jensen", "lars.jensen@hgaps.dk", 400m),
        (Guid.Parse("44444444-4444-4444-4444-444444444444"), "Mette Christoffersen", "mette.christoffersen@hgaps.dk", 325m),
        (Guid.Parse("55555555-5555-5555-5555-555555555555"), "Thomas Hansen", "thomas.hansen@hgaps.dk", 380m),
        (Guid.Parse("66666666-6666-6666-6666-666666666666"), "Sofie Madsen", "sofie.madsen@hgaps.dk", 345m),
        (Guid.Parse("77777777-7777-7777-7777-777777777777"), "Rasmus Kristensen", "rasmus.kristensen@hgaps.dk", 360m),
        (Guid.Parse("88888888-8888-8888-8888-888888888888"), "Emma Nielsen", "emma.nielsen@hgaps.dk", 335m),
        (Guid.Parse("99999999-9999-9999-9999-999999999999"), "Martin Olsen", "martin.olsen@hgaps.dk", 390m),
        (Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Frederikke Andersen", "frederikke.andersen@hgaps.dk", 355m)
    };

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var passwordHasher = new PasswordHasher<Employee>();

        foreach (var seed in SeededEmployees)
        {
            var employee = new Employee(seed.Id, seed.Name, seed.Email, seed.HourlyRate);
            var passwordHash = passwordHasher.HashPassword(employee, "HGAPS2026");

            migrationBuilder.Sql(
                $"UPDATE [Employees] SET [PasswordHash] = N'{passwordHash}' WHERE [Id] = '{seed.Id}'");
        }
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // The previous hashes are intentionally not restored because they used an unsafe custom scheme.
        foreach (var seed in SeededEmployees)
        {
            migrationBuilder.Sql(
                $"UPDATE [Employees] SET [PasswordHash] = N'' WHERE [Id] = '{seed.Id}'");
        }
    }
}
