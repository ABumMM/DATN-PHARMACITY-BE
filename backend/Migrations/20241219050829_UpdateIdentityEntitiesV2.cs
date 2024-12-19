using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityEntitiesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExportDetails_Products_ProductId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExportDetails_WarehouseExports_ExportId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExports_Warehouses_WarehouseId",
                table: "WarehouseExports");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Products_ProductId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Warehouses_WarehouseId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptDetails_Products_ProductId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptDetails_WarehouseReceipts_ReceiptId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_Suppliers_SupplierId",
                table: "WarehouseReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_Warehouses_WarehouseId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_SupplierId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_WarehouseId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptDetails_ProductId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptDetails_ReceiptId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseProducts_ProductId",
                table: "WarehouseProducts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseProducts_WarehouseId",
                table: "WarehouseProducts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExports_WarehouseId",
                table: "WarehouseExports");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExportDetails_ExportId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExportDetails_ProductId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseExports");

            migrationBuilder.DropColumn(
                name: "ExportId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WarehouseExportDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceiptDate",
                table: "WarehouseReceipts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "IdSupplier",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdSupplierNavigationId",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouse",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouseNavigationId",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseReceiptDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "IdProduct",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdProductNavigationId",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdReceipt",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdReceiptNavigationId",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseProducts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "IdProduct",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdProductNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouse",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouseNavigationId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExportDate",
                table: "WarehouseExports",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouse",
                table: "WarehouseExports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdWarehouseNavigationId",
                table: "WarehouseExports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseExportDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "IdExport",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdExportNavigationId",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdProduct",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdProductNavigationId",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_IdSupplierNavigationId",
                table: "WarehouseReceipts",
                column: "IdSupplierNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_IdWarehouseNavigationId",
                table: "WarehouseReceipts",
                column: "IdWarehouseNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptDetails_IdProductNavigationId",
                table: "WarehouseReceiptDetails",
                column: "IdProductNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptDetails_IdReceiptNavigationId",
                table: "WarehouseReceiptDetails",
                column: "IdReceiptNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_IdProductNavigationId",
                table: "WarehouseProducts",
                column: "IdProductNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_IdWarehouseNavigationId",
                table: "WarehouseProducts",
                column: "IdWarehouseNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExports_IdWarehouseNavigationId",
                table: "WarehouseExports",
                column: "IdWarehouseNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExportDetails_IdExportNavigationId",
                table: "WarehouseExportDetails",
                column: "IdExportNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExportDetails_IdProductNavigationId",
                table: "WarehouseExportDetails",
                column: "IdProductNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExportDetails_Products_IdProductNavigationId",
                table: "WarehouseExportDetails",
                column: "IdProductNavigationId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExportDetails_WarehouseExports_IdExportNavigationId",
                table: "WarehouseExportDetails",
                column: "IdExportNavigationId",
                principalTable: "WarehouseExports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExports_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseExports",
                column: "IdWarehouseNavigationId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts",
                column: "IdProductNavigationId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts",
                column: "IdWarehouseNavigationId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptDetails_Products_IdProductNavigationId",
                table: "WarehouseReceiptDetails",
                column: "IdProductNavigationId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptDetails_WarehouseReceipts_IdReceiptNavigationId",
                table: "WarehouseReceiptDetails",
                column: "IdReceiptNavigationId",
                principalTable: "WarehouseReceipts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_Suppliers_IdSupplierNavigationId",
                table: "WarehouseReceipts",
                column: "IdSupplierNavigationId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseReceipts",
                column: "IdWarehouseNavigationId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExportDetails_Products_IdProductNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExportDetails_WarehouseExports_IdExportNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseExports_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseExports");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Products_IdProductNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProducts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptDetails_Products_IdProductNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceiptDetails_WarehouseReceipts_IdReceiptNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_Suppliers_IdSupplierNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseReceipts_Warehouses_IdWarehouseNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_IdSupplierNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceipts_IdWarehouseNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptDetails_IdProductNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseReceiptDetails_IdReceiptNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseProducts_IdProductNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseProducts_IdWarehouseNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExports_IdWarehouseNavigationId",
                table: "WarehouseExports");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExportDetails_IdExportNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseExportDetails_IdProductNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "IdSupplier",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "IdSupplierNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "IdWarehouse",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "IdWarehouseNavigationId",
                table: "WarehouseReceipts");

            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "IdProductNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "IdReceipt",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "IdReceiptNavigationId",
                table: "WarehouseReceiptDetails");

            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdProductNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdWarehouse",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdWarehouseNavigationId",
                table: "WarehouseProducts");

            migrationBuilder.DropColumn(
                name: "IdWarehouse",
                table: "WarehouseExports");

            migrationBuilder.DropColumn(
                name: "IdWarehouseNavigationId",
                table: "WarehouseExports");

            migrationBuilder.DropColumn(
                name: "IdExport",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "IdExportNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "IdProduct",
                table: "WarehouseExportDetails");

            migrationBuilder.DropColumn(
                name: "IdProductNavigationId",
                table: "WarehouseExportDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReceiptDate",
                table: "WarehouseReceipts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierId",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "WarehouseReceipts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseReceiptDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiptId",
                table: "WarehouseReceiptDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "WarehouseProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExportDate",
                table: "WarehouseExports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "WarehouseExports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "WarehouseExportDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExportId",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "WarehouseExportDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_SupplierId",
                table: "WarehouseReceipts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceipts_WarehouseId",
                table: "WarehouseReceipts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptDetails_ProductId",
                table: "WarehouseReceiptDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseReceiptDetails_ReceiptId",
                table: "WarehouseReceiptDetails",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_ProductId",
                table: "WarehouseProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_WarehouseId",
                table: "WarehouseProducts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExports_WarehouseId",
                table: "WarehouseExports",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExportDetails_ExportId",
                table: "WarehouseExportDetails",
                column: "ExportId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseExportDetails_ProductId",
                table: "WarehouseExportDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExportDetails_Products_ProductId",
                table: "WarehouseExportDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExportDetails_WarehouseExports_ExportId",
                table: "WarehouseExportDetails",
                column: "ExportId",
                principalTable: "WarehouseExports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseExports_Warehouses_WarehouseId",
                table: "WarehouseExports",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Products_ProductId",
                table: "WarehouseProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProducts_Warehouses_WarehouseId",
                table: "WarehouseProducts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptDetails_Products_ProductId",
                table: "WarehouseReceiptDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceiptDetails_WarehouseReceipts_ReceiptId",
                table: "WarehouseReceiptDetails",
                column: "ReceiptId",
                principalTable: "WarehouseReceipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_Suppliers_SupplierId",
                table: "WarehouseReceipts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseReceipts_Warehouses_WarehouseId",
                table: "WarehouseReceipts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
