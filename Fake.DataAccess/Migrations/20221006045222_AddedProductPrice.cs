using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fake.DataAccess.Migrations
{
    public partial class AddedProductPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ListPrice",
                table: "Products",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "ListPrice");
        }
    }
}
