using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly FinalContext db;

        public WarehouseController(FinalContext _db)
        {
            db = _db;
        }

        // Lấy tất cả kho
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Warehouses>>> GetAllWarehouses()
        {
            if (db.Warehouses == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from warehouse in db.Warehouses
                        orderby warehouse.Name
                        select new
                        {
                            warehouse.Id,
                            warehouse.Name,
                            warehouse.Address
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        // Lấy kho theo Id
        [HttpGet]
        public async Task<ActionResult<Warehouses>> GetWarehouseById(Guid id)
        {
            var warehouse = await db.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return Ok(new
                {
                    message = "Không tìm thấy kho!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = warehouse
            });
        }

        // Thêm mới kho
        [HttpPost("add")]
        public async Task<ActionResult> CreateWarehouse(Warehouses warehouse)
        {
            if (db.Warehouses == null)
            {
                return BadRequest(new { message = "Không thể thêm kho!" });
            }

            db.Warehouses.Add(warehouse);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm kho thành công!",
                status = 200
            });
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] Warehouses warehouse)
        {
            var _warehouse = await db.Warehouses.FindAsync(warehouse.Id);
            if (_warehouse == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.Warehouses.FirstOrDefaultAsync(x => x.Id == warehouse.Id)).CurrentValues.SetValues(warehouse);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            if (db.Warehouses == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _warehouse = await db.Warehouses.FindAsync(id);
            if (_warehouse == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.Warehouses.Remove(_warehouse);
                await db.SaveChangesAsync();
                return Ok(new
                {
                    message = "Xóa thành công!",
                    status = 200
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    message = "Lỗi rồi!",
                    status = 400,
                    data = e.Message
                });
            }
        }

    }
}
