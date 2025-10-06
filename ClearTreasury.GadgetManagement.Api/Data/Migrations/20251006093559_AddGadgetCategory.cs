using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearTreasury.GadgetManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGadgetCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryGadget",
                schema: "inventory",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGadget", x => new { x.CategoriesId, x.GadgetId });
                    table.ForeignKey(
                        name: "FK_CategoryGadget_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalSchema: "inventory",
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryGadget_Gadget_GadgetId",
                        column: x => x.GadgetId,
                        principalSchema: "inventory",
                        principalTable: "Gadget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                schema: "inventory",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryGadget_GadgetId",
                schema: "inventory",
                table: "CategoryGadget",
                column: "GadgetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryGadget",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "inventory");
        }
    }
}
