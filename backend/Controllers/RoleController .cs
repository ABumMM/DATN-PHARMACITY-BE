 using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace backend.Controllers 
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly FinalContext db;
        public RoleController(FinalContext _db)
        {
            db = _db;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetAllRole()
        {
            if (db.ApplicationRoles == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.ApplicationRoles.ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRole(Guid id)
        {
            if (db.ApplicationRoles == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.ApplicationRoles.Where(x => x.Id == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpPost("add")]

        public async Task<ActionResult> AddRole([FromBody] Roles role)
        {
            var _role = await db.ApplicationRoles.FirstOrDefaultAsync(x => String.Compare(x.Name, role.Name,StringComparison.OrdinalIgnoreCase) == 0);
            if (_role != null)
            {
                return Ok(new
                {
                    message = "Role đã tồn tại!",
                    status = 400
                });
            }
            await db.ApplicationRoles.AddAsync(role);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = role
            });
        }
        [HttpPut("edit")]

        public async Task<ActionResult> Edit([FromBody] Roles role)
        {
            var _role = await db.ApplicationRoles.FindAsync(role.Id);
            if (_role == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.ApplicationRoles.FirstOrDefaultAsync(x => x.Id == role.Id)).CurrentValues.SetValues(role);
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
            if (db.ApplicationRoles == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _role = await db.ApplicationRoles.FindAsync(id);
            if (_role == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.ApplicationRoles.Remove(_role);
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
