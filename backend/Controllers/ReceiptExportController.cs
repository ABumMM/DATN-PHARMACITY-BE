using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/receiptexport")]
    [ApiController]
    public class ReceiptExportController : ControllerBase
    {
        private readonly FinalContext db;

        public ReceiptExportController(FinalContext _db)
        {
            db = _db;
        }

        // Tạo phiếu nhập kho
        [HttpPost("receipt")]
        public async Task<ActionResult> CreateReceipt([FromBody] WarehouseReceipts receipt)
        {
            if (receipt == null || !receipt.ReceiptDetails.Any())
                return BadRequest(new { message = "Thông tin phiếu nhập không hợp lệ." });

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var warehouse = await db.Warehouses.FindAsync(receipt.IdWarehouse);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                var supplier = await db.Suppliers.FindAsync(receipt.IdSupplier);
                if (supplier == null)
                    return NotFound(new { message = "Nhà cung cấp không tồn tại." });

                db.WarehouseReceipts.Add(receipt);

                foreach (var detail in receipt.ReceiptDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.IdWarehouse == receipt.IdWarehouse && wp.IdProduct == detail.IdProduct);

                    if (warehouseProduct == null)
                    {
                        // Nếu sản phẩm chưa có trong kho, thêm sản phẩm mới
                        db.WarehouseProducts.Add(new WarehouseProducts
                        {
                            Id = Guid.NewGuid(),
                            IdWarehouse = receipt.IdWarehouse,
                            IdProduct = detail.IdProduct,
                            Quantity = detail.Quantity,
                            ExpirationDate = detail.ExpirationDate // Kiểm tra ExpirationDate nullable
                        });
                    }
                    else
                    {
                        if (warehouseProduct.ExpirationDate != detail.ExpirationDate)
                        {
                            // Nếu ngày hết hạn khác nhau, thêm sản phẩm mới với ngày hết hạn mới
                            db.WarehouseProducts.Add(new WarehouseProducts
                            {
                                Id = Guid.NewGuid(),
                                IdWarehouse = receipt.IdWarehouse,
                                IdProduct = detail.IdProduct,
                                Quantity = detail.Quantity,
                                ExpirationDate = detail.ExpirationDate // Kiểm tra ExpirationDate nullable
                            });
                        }
                        else
                        {
                            // Cộng thêm số lượng vào kho nếu ngày hết hạn trùng
                            warehouseProduct.Quantity += detail.Quantity;
                        }
                    }
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Tạo phiếu nhập kho thành công!",
                    status = 200,
                    data = new
                    {
                        receiptId = receipt.Id,
                        warehouseName = warehouse.Name,
                        supplierName = supplier.Name,
                        receiptDetails = receipt.ReceiptDetails
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }


        // Tạo phiếu xuất kho
        [HttpPost("export")]
        public async Task<ActionResult> CreateExport([FromBody] WarehouseExports export)
        {
            if (export == null || !export.ExportDetails.Any())
                return BadRequest(new { message = "Thông tin phiếu xuất không hợp lệ." });

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var warehouse = await db.Warehouses.FindAsync(export.IdWarehouse);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                db.WarehouseExports.Add(export);

                foreach (var detail in export.ExportDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.IdWarehouse == export.IdWarehouse && wp.IdProduct == detail.IdProduct);

                    if (warehouseProduct == null || warehouseProduct.Quantity < detail.Quantity)
                        return BadRequest(new { message = $"Không đủ số lượng sản phẩm (ID: {detail.IdProduct}) trong kho để xuất." });

                    warehouseProduct.Quantity -= detail.Quantity;
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Tạo phiếu xuất kho thành công!",
                    status = 200,
                    data = new
                    {
                        exportId = export.Id,
                        warehouseName = warehouse.Name,
                        exportDetails = export.ExportDetails
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }


        // Lấy tất cả phiếu nhập kho
        [HttpGet("allreceipt")]
        public async Task<ActionResult> GetAllReceipts()
        {
            var receipts = await db.WarehouseReceipts
                .Include(r => r.ReceiptDetails)
                .ThenInclude(rd => rd.IdProductNavigation)
                .Include(r => r.IdWarehouseNavigation)
                .Include(r => r.IdSupplierNavigation)
                .ToListAsync();

            if (!receipts.Any())
                return NotFound(new { message = "Không có dữ liệu phiếu nhập kho." });

            return Ok(new
            {
                message = "Lấy danh sách phiếu nhập kho thành công!",
                status = 200,
                data = receipts.Select(receipt => new
                {
                    receiptId = receipt.Id,
                    warehouseName = receipt.IdWarehouseNavigation.Name,
                    supplierName = receipt.IdSupplierNavigation.Name,
                    receiptDetails = receipt.ReceiptDetails.Select(rd => new
                    {
                        productId = rd.IdProduct,
                        productName = rd.IdProductNavigation.Name,
                        quantity = rd.Quantity,
                        expirationDate = rd.ExpirationDate?.ToString("yyyy-MM-dd") // Nullable DateTime
                    })
                })
            });
        }


        // Lấy tất cả phiếu xuất kho
        [HttpGet("allexport")]
        public async Task<ActionResult> GetAllExports()
        {
            var exports = await db.WarehouseExports
                .Include(e => e.ExportDetails)
                .ThenInclude(ed => ed.IdProductNavigation)
                .Include(e => e.IdWarehouseNavigation)
                .ToListAsync();

            if (!exports.Any())
                return NotFound(new { message = "Không có dữ liệu phiếu xuất kho." });

            return Ok(new
            {
                message = "Lấy danh sách phiếu xuất kho thành công!",
                status = 200,
                data = exports.Select(export => new
                {
                    exportId = export.Id,
                    warehouseName = export.IdWarehouseNavigation.Name,
                    exportDetails = export.ExportDetails.Select(ed => new
                    {
                        productId = ed.IdProduct,
                        productName = ed.IdProductNavigation.Name,
                        quantity = ed.Quantity
                    })
                })
            });
        }

    }
}
