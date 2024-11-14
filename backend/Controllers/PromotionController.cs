using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly FinalContext db;

        public PromotionController(FinalContext _db)
        {
            db = _db;
        }


        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Promotions>>> GetAllPromotion()
        {
            if (db.Promotions == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = await db.Promotions.ToListAsync();

            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotions>>> GetPromotion(Guid id)
        {
            if (db.Promotions == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = await db.Promotions.Where(x => x.Id == id).ToListAsync();
            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 404
                });
            }

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPromotion([FromBody] Promotions promotion)
        {
            var _promotionExists = await db.Promotions
                .AnyAsync(x => x.Name.ToLower() == promotion.Name.ToLower());

            if (_promotionExists)
            {
                return BadRequest(new
                {
                    message = "Tạo thất bại! Promotion đã tồn tại.",
                    status = 400,
                });
            }

            await db.Promotions.AddAsync(promotion);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo thành công! Promotion đã được thêm.",
                status = 200,
                data = promotion
            });
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] Promotions promotion)
        {
            var _promotion = await db.Promotions.FindAsync(promotion.Id);
            if (_promotion == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }

            db.Entry(_promotion).CurrentValues.SetValues(promotion);
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
            if (db.Promotions == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _promotion = await db.Promotions.FindAsync(id);
            if (_promotion == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 404
                });
            }

            try
            {
                db.Promotions.Remove(_promotion);
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
                    message = "Lỗi khi xóa dữ liệu!",
                    status = 400,
                    error = e.Message
                });
            }
        }
    }
}
