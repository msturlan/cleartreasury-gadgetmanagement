using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearTreasury.GadgetManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGadgetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "Gadget",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gadget", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gadget_DateCreated",
                schema: "inventory",
                table: "Gadget",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_Gadget_Name",
                schema: "inventory",
                table: "Gadget",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gadget",
                schema: "inventory");
        }
    }
}
