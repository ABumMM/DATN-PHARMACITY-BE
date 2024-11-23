using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly FinalContext db;

        public SupplierController(FinalContext _db)
        {
            db = _db;
        }

        // Lấy tất cả nhà cung cấp
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Suppliers>>> GetAllSuppliers()
        {
            if (db.Suppliers == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from supplier in db.Suppliers
                        orderby supplier.Name
                        select new
                        {
                            supplier.Id,
                            supplier.Name,
                            supplier.Address,
                            supplier.Phone,
                            supplier.Email
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        // Lấy nhà cung cấp theo Id
        [HttpGet]
        public async Task<ActionResult<Suppliers>> GetSupplierById(Guid id)
        {
            var supplier = await db.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return Ok(new
                {
                    message = "Không tìm thấy nhà cung cấp!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = supplier
            });
        }

        // Thêm mới nhà cung cấp
        [HttpPost("add")]
        public async Task<ActionResult> CreateSupplier(Suppliers supplier)
        {
            if (db.Suppliers == null)
            {
                return BadRequest(new { message = "Không thể thêm nhà cung cấp!" });
            }

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm nhà cung cấp thành công!",
                status = 200
            });
        }

        // Cập nhật thông tin nhà cung cấp
        [HttpPut("edit")]
        public async Task<ActionResult> UpdateSupplier(Guid id, Suppliers supplier)
        {
            var existingSupplier = await db.Suppliers.FindAsync(id);

            if (existingSupplier == null)
            {
                return NotFound(new { message = "Nhà cung cấp không tồn tại!" });
            }

            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Phone = supplier.Phone;
            existingSupplier.Email = supplier.Email;

            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật nhà cung cấp thành công!",
                status = 200
            });
        }

        // Xóa nhà cung cấp
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteSupplier(Guid id)
        {
            var supplier = await db.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound(new { message = "Không tìm thấy nhà cung cấp để xóa!" });
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa nhà cung cấp thành công!",
                status = 200
            });
        }
    }
}
