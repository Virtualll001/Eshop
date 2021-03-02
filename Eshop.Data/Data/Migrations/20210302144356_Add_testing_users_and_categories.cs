using Microsoft.EntityFrameworkCore.Migrations;

namespace Eshop.Data.Migrations
{
    public partial class Add_testing_users_and_categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[] { 1, false, 1, null, "Oblečení pro panenky 30cm", "oblecky-pro-panenky-30cm" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[] { 2, false, 4, null, "Oblečení pro panenky 36cm", "oblecky-pro-panenky-36cm" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Hidden", "OrderNo", "ParentCategoryId", "Title", "Url" },
                values: new object[,]
                {
                    { 3, false, 2, 1, "Šaty pro panenku 30cm", "saty-pro-panenku-30cm" },
                    { 4, false, 3, 1, "Sukně pro panenku 30cm", "sukne-pro-panenku-30cm" },
                    { 5, false, 5, 2, "Trička pro panenku 36cm", "tricka-pro-panenku-36cm" },
                    { 6, false, 6, 2, "Kraťasy pro panenku 36cm", "kratasy-pro-panenku-36cm" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);
        }
    }
}
