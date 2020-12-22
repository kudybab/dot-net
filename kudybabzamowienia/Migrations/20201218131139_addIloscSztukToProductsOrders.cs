using Microsoft.EntityFrameworkCore.Migrations;

namespace kudybabzamowienia.Migrations
{
    public partial class addIloscSztukToProductsOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ilosc_sztuk",
                table: "ProductOrders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ilosc_sztuk",
                table: "ProductOrders");
        }
    }
}
