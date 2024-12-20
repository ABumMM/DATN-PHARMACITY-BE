using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseProductController : ControllerBase
    {
        private readonly FinalContext db;

        public WarehouseProductController(FinalContext _db)
        {
            db = _db;
        }

        // Lấy danh sách tất cả sản phẩm trong kho
        [HttpGet("allwarehouseproducts")]
        public async Task<IActionResult> GetAllWarehouseProducts()
        {
            var warehouseProducts = await db.WarehouseProducts
                .Include(wp => wp.IdWarehouseNavigation)
                .Include(wp => wp.IdProductNavigation)
                .ToListAsync();

            var result = warehouseProducts.Select(wp => new
            {
                WarehouseProductId = wp.Id,
                WarehouseId = wp.IdWarehouse,
                WarehouseName = wp.IdWarehouseNavigation?.Name,
                ProductId = wp.IdProduct,
                ProductName = wp.IdProductNavigation?.Name,
                SupplierId = wp.IdSupplier,
                SupplierName = wp.IdSupplierNavigation?.Name,
                Quantity = wp.Quantity,
                ExpirationDate = wp.ExpirationDate.ToString("yyyy-MM-dd")
            });

            return Ok(new
            {
                message = "Lấy danh sách sản phẩm trong kho thành công!",
                status = 200,
                data = result
            });
        }

        // Xóa sản phẩm khỏi kho
        [HttpDelete("deletewarehouseproduct/{id}")]
        public async Task<IActionResult> DeleteWarehouseProduct(Guid id)
        {
            try
            {
                var warehouseProduct = await db.WarehouseProducts.FindAsync(id);
                if (warehouseProduct == null)
                    return NotFound(new { message = "Không tìm thấy sản phẩm cần xóa." });

                // Kiểm tra xem sản phẩm có trong phiếu nhập/xuất nào không
                var isInReceipt = await db.WarehouseReceiptDetails.AnyAsync(d => d.IdProduct == warehouseProduct.IdProduct);
                var isInExport = await db.WarehouseExportDetails.AnyAsync(d => d.IdProduct == warehouseProduct.IdProduct);

                if (isInReceipt || isInExport)
                {
                    return BadRequest(new { message = "Sản phẩm đã có trong phiếu nhập/xuất, không thể xóa." });
                }

                db.WarehouseProducts.Remove(warehouseProduct);
                await db.SaveChangesAsync();
                return Ok(new { message = "Xóa sản phẩm khỏi kho thành công!", status = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }
    }
}