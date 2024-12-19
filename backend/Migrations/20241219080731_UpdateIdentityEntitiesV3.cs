using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityEntitiesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdWarehouseNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdProductNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "IdSupplier",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdSupplierNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_IdSupplierNavigationId",
                table: "WarehouseProducts",
                column: "IdSupplierNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts",
                column: "IdProductNavigationId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Suppliers_IdSupplierNavigationId",
                table: "WarehouseProducts",
                column: "IdSupplierNavigationId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts",
                column: "IdWarehouseNavigationId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Suppliers_IdSupplierNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseProducts_IdSupplierNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdSupplier",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdSupplierNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdWarehouseNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IdProductNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts",
                column: "IdProductNavigationId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts",
                column: "IdWarehouseNavigationId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
    