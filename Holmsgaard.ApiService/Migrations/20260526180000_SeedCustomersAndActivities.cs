using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holmsgaard.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class SeedCustomersAndActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed 5 customers
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Name", "Email", "Phone", "IsActive" },
                values: new object[,]
                {
                    { Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Niels Bakgaard",       "niels.bakgaard@erhverv.dk",    "+45 12345678", true },
                    { Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Bolette Vandt",         "bolette.vandt@byggevarer.dk",  "+45 23456789", true },
                    { Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Ronni Helmersen",      "ronni.helmersen@elektro.dk",   "+45 34567890", true },
                    { Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Hanne Schmidtgaard",   "hanne.schmidtgaard@finish.dk",  "+45 45678901", true },
                    { Guid.Parse("00000000-0000-0000-0000-000000000001"), "Flex Rohr",             "kontakt@flexrohr.dk",           "+45 56789012", true },
                });

            // Seed 10 activities (2 per customer)
            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "CustomerId", "Title", "Description", "IsActive" },
                values: new object[,]
                {
                    // Customer 1: Niels Bakgaard
                    { Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Tagejendom Renovering",     "Renovering af 2. sal på kundeejendom",        true },
                    { Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"), Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Faldstammer Installation",  "Udskiftning af faldstammer i kælder",          true },
                    // Customer 2: Bolette Vandt
                    { Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"), Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Nybyggeri Badeværelse",     "Installationsopgaver til nybyggeri",          true },
                    { Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), "VVS Projekt",               "Samlet VVS-arbejde for kontorbygning",         true },
                    // Customer 3: Ronni Helmersen
                    { Guid.Parse("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"), Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Elektrisk Installation",    "Komplet el-installation i boligblok",          true },
                    { Guid.Parse("c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2"), Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Belysningsanlæg",           "LED-belysningsanlæg til erhvervsbygning",      true },
                    // Customer 4: Hanne Schmidtgaard
                    { Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"), Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Overfladebehandling",       "Maling og overfladebehandling af lokaler",      true },
                    { Guid.Parse("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"), Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Gulvbelægning",             "Installation af nye gulve i trappeopgange",      true },
                    // Customer 5: Flex Rohr
                    { Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"), Guid.Parse("00000000-0000-0000-0000-000000000001"), "Rørarbejde Projekt",        "Stort rørinstallation projekt på fabrik",      true },
                    { Guid.Parse("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"), Guid.Parse("00000000-0000-0000-0000-000000000001"), "Kloakarbejde",              "Udvidelse af kloaksystem på industrigrund",   true },
                });

            // Seed 5 products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Sku", "UnitPrice", "IsActive", "StockQuantity" },
                values: new object[,]
                {
                    { Guid.Parse("11111111-0000-0000-0000-000000000001"), "Aftagelig Pakning 1/2\"",  "VVS-PAK-012",  15.00m,  true, 100 },
                    { Guid.Parse("11111111-0000-0000-0000-000000000002"), "Kobberrør 15mm (m)",        "RØR-KOB-015",  45.00m,  true, 200 },
                    { Guid.Parse("11111111-0000-0000-0000-000000000003"), "Aftagelig Pakning 1\"",      "VVS-PAK-025",  22.00m,  true, 100 },
                    { Guid.Parse("11111111-0000-0000-0000-000000000004"), "Kabeltromle 50m",          "EL-KAB-050",   350.00m, true, 25 },
                    { Guid.Parse("11111111-0000-0000-0000-000000000005"), "LED Armatur 60cm",           "EL-LED-060",   280.00m, true, 50 },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove customers ( cascades to activities)
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"));
            migrationBuilder.DeleteData(table: "Activities", keyColumn: "Id", keyValue: Guid.Parse("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"));
            migrationBuilder.DeleteData(table: "Customers", keyColumn: "Id", keyValue: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"));
            migrationBuilder.DeleteData(table: "Customers", keyColumn: "Id", keyValue: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
            migrationBuilder.DeleteData(table: "Customers", keyColumn: "Id", keyValue: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));
            migrationBuilder.DeleteData(table: "Customers", keyColumn: "Id", keyValue: Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));
            migrationBuilder.DeleteData(table: "Customers", keyColumn: "Id", keyValue: Guid.Parse("00000000-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: Guid.Parse("11111111-0000-0000-0000-000000000001"));
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: Guid.Parse("11111111-0000-0000-0000-000000000002"));
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: Guid.Parse("11111111-0000-0000-0000-000000000003"));
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: Guid.Parse("11111111-0000-0000-0000-000000000004"));
            migrationBuilder.DeleteData(table: "Products", keyColumn: "Id", keyValue: Guid.Parse("11111111-0000-0000-0000-000000000005"));
        }
    }
}
