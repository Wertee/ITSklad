using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sklad.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Products",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Applicant = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CurrentCount = table.Column<int>(type: "int", nullable: false),
            //         CanBeGiven = table.Column<bool>(type: "bit", nullable: false),
            //         CountToGive = table.Column<int>(type: "int", nullable: false),
            //         YearOfIncome = table.Column<int>(type: "int", nullable: false),
            //         Used = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Products", x => x.Id);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "Incomes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         ProductId = table.Column<int>(type: "int", nullable: false),
            //         Count = table.Column<int>(type: "int", nullable: false),
            //         Date = table.Column<DateTime>(type: "datetime2", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Incomes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Incomes_Products_ProductId",
            //             column: x => x.ProductId,
            //             principalTable: "Products",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });
            //
            // migrationBuilder.CreateTable(
            //     name: "Outcomes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ProductId = table.Column<int>(type: "int", nullable: false),
            //         Count = table.Column<int>(type: "int", nullable: false),
            //         Date = table.Column<DateTime>(type: "datetime2", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Outcomes", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Outcomes_Products_ProductId",
            //             column: x => x.ProductId,
            //             principalTable: "Products",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Incomes_ProductId",
            //     table: "Incomes",
            //     column: "ProductId");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Outcomes_ProductId",
            //     table: "Outcomes",
            //     column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Outcomes");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
