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
                var warehouse = await db.Warehouses.FindAsync(receipt.WarehouseId);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                var supplier = await db.Suppliers.FindAsync(receipt.SupplierId);
                if (supplier == null)
                    return NotFound(new { message = "Nhà cung cấp không tồn tại." });

                db.WarehouseReceipts.Add(receipt);

                foreach (var detail in receipt.ReceiptDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.WarehouseId == receipt.WarehouseId && wp.ProductId == detail.ProductId);

                    if (warehouseProduct == null)
                    {
                        // Nếu không có sản phẩm trong kho, tạo một lô mới cho sản phẩm với hạn sử dụng
                        db.WarehouseProducts.Add(new WarehouseProducts
                        {
                            Id = Guid.NewGuid(),
                            WarehouseId = receipt.WarehouseId,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            ExpirationDate = detail.ExpirationDate
                        });
                    }
                    else
                    {
                        // Nếu sản phẩm đã có trong kho, kiểm tra hạn sử dụng có khác không
                        if (warehouseProduct.ExpirationDate != detail.ExpirationDate)
                        {
                            // Nếu khác, tạo một lô mới với hạn sử dụng mới
                            db.WarehouseProducts.Add(new WarehouseProducts
                            {
                                Id = Guid.NewGuid(),
                                WarehouseId = receipt.WarehouseId,
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                ExpirationDate = detail.ExpirationDate
                            });
                        }
                        else
                        {
                            // Nếu cùng hạn sử dụng, cộng dồn số lượng vào lô cũ
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
                var warehouse = await db.Warehouses.FindAsync(export.WarehouseId);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                db.WarehouseExports.Add(export);

                foreach (var detail in export.ExportDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.WarehouseId == export.WarehouseId && wp.ProductId == detail.ProductId);

                    if (warehouseProduct == null || warehouseProduct.Quantity < detail.Quantity)
                        return BadRequest(new { message = $"Không đủ số lượng sản phẩm (ID: {detail.ProductId}) trong kho để xuất." });

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
                .ThenInclude(rd => rd.Product)
                .Include(r => r.Warehouse)
                .Include(r => r.Supplier)
                .ToListAsync();

            if (!receipts.Any())
                return NotFound(new { message = "Không có dữ liệu phiếu nhập kho." });

            return Ok(new
            {
                message = "Lấy danh sách phiếu nhập kho thành công!",
                status = 200,
                data = receipts.Select(receipt => new {
                    receiptId = receipt.Id,
                    warehouseName = receipt.Warehouse.Name,
                    supplierName = receipt.Supplier.Name,
                    receiptDetails = receipt.ReceiptDetails
                })
            });
        }

        // Lấy tất cả phiếu xuất kho
        [HttpGet("allexport")]
        public async Task<ActionResult> GetAllExports()
        {
            var exports = await db.WarehouseExports
                .Include(e => e.ExportDetails)
                .ThenInclude(ed => ed.Product)
                .Include(e => e.Warehouse)
                .ToListAsync();

            if (!exports.Any())
                return NotFound(new { message = "Không có dữ liệu phiếu xuất kho." });

            return Ok(new
            {
                message = "Lấy danh sách phiếu xuất kho thành công!",
                status = 200,
                data = exports.Select(export => new {
                    exportId = export.Id,
                    warehouseName = export.Warehouse.Name,
                    exportDetails = export.ExportDetails
                })
            });
        }
    }
}
