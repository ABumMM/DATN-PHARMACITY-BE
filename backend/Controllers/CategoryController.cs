﻿using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly FinalContext db;
        public CategoryController(FinalContext _db)
        { 
            db = _db;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Categories>>> GetAllCategory(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                return BadRequest(new
                {
                    message = "Số trang phải lớn hơn 0!",
                    status = 400
                });
            }

            if (pageSize < 1)
            {
                return BadRequest(new
                {
                    message = "Kích thước trang phải lớn hơn 0!",
                    status = 400
                });
            }
            if (db.Categories == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Categories.ToListAsync();
            if (!_data.Any())
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var skip = (pageNumber - 1) * pageSize;

            var totalRecords = db.Categories.Count();

            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data,
                pagination = new
                {
                    currentPage = pageNumber, pageSize, totalRecords,
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                }
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategory(Guid id)
        {
            if (db.Categories == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Categories.Where(x => x.Id == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCategory([FromBody] Categories category)
        {
            var _category = await db.Categories.Where(x=>x.Slug.Equals(category.Slug)).ToListAsync();
            if(_category.Count != 0)
            {
                return Ok(new
                {
                    message = "Tạo thất bại!",
                    status = 400,
                });
            }
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = category
            });
        }

        [HttpPut("edit")]

        public async Task<ActionResult> Edit([FromBody]Categories category)
        {
            var _category = await db.Categories.FindAsync(category.Id);
            if (_category == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.Categories.FirstOrDefaultAsync(x => x.Id == category.Id)).CurrentValues.SetValues(category);
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
            if (db.Categories == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _category = await db.Categories.FindAsync(id);
            if (_category == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.Categories.Remove(_category);
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
