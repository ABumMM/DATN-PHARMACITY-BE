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

        [HttpPost("receipt")]
        public async Task<ActionResult> CreateReceipt([FromBody] WarehouseReceipts receipt)
        {
            if (receipt == null || !receipt.ReceiptDetails.Any())
            {
                return BadRequest(new { message = "Thông tin phiếu nhập không hợp lệ." });
            }

            try
            {
                // Kiểm tra WarehouseId có tồn tại
                var warehouse = await db.Warehouses.FindAsync(receipt.WarehouseId);
                if (warehouse == null)
                {
                    return NotFound(new { message = "Kho hàng không tồn tại." });
                }

                // Kiểm tra SupplierId có tồn tại
                var supplier = await db.Suppliers.FindAsync(receipt.SupplierId);
                if (supplier == null)
                {
                    return NotFound(new { message = "Nhà cung cấp không tồn tại." });
                }

                // Thêm phiếu nhập kho
                db.WarehouseReceipts.Add(receipt);

                // Cập nhật số lượng sản phẩm trong kho
                foreach (var detail in receipt.ReceiptDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.WarehouseId == receipt.WarehouseId && wp.ProductId == detail.ProductId);

                    if (warehouseProduct == null)
                    {
                        // Nếu sản phẩm chưa có trong kho, thêm mới
                        db.WarehouseProducts.Add(new WarehouseProducts
                        {
                            Id = Guid.NewGuid(),
                            WarehouseId = receipt.WarehouseId,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity
                        });
                    }
                    else
                    {
                        // Nếu sản phẩm đã có, cập nhật số lượng
                        warehouseProduct.Quantity += detail.Quantity;
                    }
                }

                await db.SaveChangesAsync();

                return Ok(new { message = "Tạo phiếu nhập kho thành công!", status = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost("export")]
        public async Task<ActionResult> CreateExport([FromBody] WarehouseExports export)
        {
            if (export == null || !export.ExportDetails.Any())
            {
                return BadRequest(new { message = "Thông tin phiếu xuất không hợp lệ." });
            }

            try
            {
                // Kiểm tra WarehouseId có tồn tại
                var warehouse = await db.Warehouses.FindAsync(export.WarehouseId);
                if (warehouse == null)
                {
                    return NotFound(new { message = "Kho hàng không tồn tại." });
                }

                // Thêm phiếu xuất kho
                db.WarehouseExports.Add(export);

                // Cập nhật số lượng sản phẩm trong kho
                foreach (var detail in export.ExportDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.WarehouseId == export.WarehouseId && wp.ProductId == detail.ProductId);

                    if (warehouseProduct == null || warehouseProduct.Quantity < detail.Quantity)
                    {
                        return BadRequest(new { message = $"Không đủ số lượng sản phẩm (ID: {detail.ProductId}) trong kho để xuất." });
                    }

                    // Cập nhật số lượng
                    warehouseProduct.Quantity -= detail.Quantity;
                }

                await db.SaveChangesAsync();

                return Ok(new { message = "Tạo phiếu xuất kho thành công!", status = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra: " + ex.Message });
            }
        }


        [HttpGet("allreceipt")]
        public async Task<ActionResult> GetAllReceipts()
        {
            if (db.WarehouseReceipts == null)
            {
                return NotFound(new { message = "Không có dữ liệu phiếu nhập kho." });
            }

            var receipts = await db.WarehouseReceipts
                .Include(r => r.ReceiptDetails)
                .ToListAsync();

            return Ok(new
            {
                message = "Lấy danh sách phiếu nhập kho thành công!",
                status = 200,
                data = receipts
            });
        }

        [HttpGet("allexport")]
        public async Task<ActionResult> GetAllExports()
        {
            if (db.WarehouseExports == null)
            {
                return NotFound(new { message = "Không có dữ liệu phiếu xuất kho." });
            }

            var exports = await db.WarehouseExports
                .Include(e => e.ExportDetails)
                .ToListAsync();

            return Ok(new
            {
                message = "Lấy danh sách phiếu xuất kho thành công!",
                status = 200,
                data = exports
            });
        }
    }
}
