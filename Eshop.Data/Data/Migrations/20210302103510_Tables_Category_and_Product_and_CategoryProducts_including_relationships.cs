﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Eshop.Data.Migrations
{
    public partial class Tables_Category_and_Product_and_CategoryProducts_including_relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    OrderNo = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false),
                    ParentCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 255, nullable: false),
                    Url = table.Column<string>(maxLength: 255, nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    ShortDescription = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,1)", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(10,1)", nullable: true),
                    Stock = table.Column<int>(nullable: false),
                    ImagesCount = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "CategoryProduct",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryProduct", x => new { x.CategoryId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_ProductId",
                table: "CategoryProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryProduct");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
