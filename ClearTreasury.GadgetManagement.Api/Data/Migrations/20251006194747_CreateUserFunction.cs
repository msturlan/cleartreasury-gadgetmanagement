using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClearTreasury.GadgetManagement.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE FUNCTION dbo.fn_Generate3grams(@input NVARCHAR(MAX))
                RETURNS NVARCHAR(MAX)
                WITH SCHEMABINDING
                AS
                BEGIN
                    DECLARE @i INT = 1;
                	DECLARE @n INT = 3;
                	DECLARE @len INT = LEN(@input);
                	DECLARE @result NVARCHAR(MAX) = '';

                    WHILE @i <= @len - @n + 1
                    BEGIN
                        SET @result = @result + SUBSTRING(@input, @i, @n) + ' ';
                        SET @i = @i + 1;
                    END
                    RETURN TRIM(@result);
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP FUNCTION dbo.fn_Generate3grams
                """);
        }
    }
}
