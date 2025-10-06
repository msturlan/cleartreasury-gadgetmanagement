using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearTreasury.GadgetManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGadgetGeneratedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameGrams",
                schema: "inventory",
                table: "Gadget",
                type: "nvarchar(max)",
                nullable: false,
                computedColumnSql: "dbo.fn_Generate3grams(Name)",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameGrams",
                schema: "inventory",
                table: "Gadget");
        }
    }
}
