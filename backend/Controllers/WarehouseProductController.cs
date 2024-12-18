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
                .Include(wp => wp.Warehouse)
                .Include(wp => wp.Product)
                .ToListAsync();

            var result = warehouseProducts.Select(wp => new
            {
                WarehouseProductId = wp.Id,
                WarehouseId = wp.WarehouseId,
                WarehouseName = wp.Warehouse.Name,
                ProductId = wp.ProductId,
                ProductName = wp.Product.Name,
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
