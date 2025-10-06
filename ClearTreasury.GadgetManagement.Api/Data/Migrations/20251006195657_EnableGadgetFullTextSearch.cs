using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearTreasury.GadgetManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnableGadgetFullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;

                CREATE FULLTEXT INDEX ON {DbSchemas.Inventory}.{nameof(Gadget)}
                (
                    {nameof(Gadget.NameGrams)} LANGUAGE 1033
                )
                KEY INDEX PK_{nameof(Gadget)}
                ON ftCatalog
                WITH CHANGE_TRACKING AUTO;
                """,
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"""
                DROP FULLTEXT INDEX ON {DbSchemas.Inventory}.{nameof(Gadget)};

                DROP FULLTEXT CATALOG ftCatalog;
                """,
                suppressTransaction: true);
        }
    }
}
