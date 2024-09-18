using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.Migrations
{
    /// <inheritdoc />
    public partial class Pricestore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceProduct",
                table: "StoreProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceProduct",
                table: "StoreProducts");
        }
    }
}
