using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.Migrations
{
    /// <inheritdoc />
    public partial class addismain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceProduct",
                table: "StoreProducts");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Stores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Stores");

            migrationBuilder.AddColumn<decimal>(
                name: "PriceProduct",
                table: "StoreProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
