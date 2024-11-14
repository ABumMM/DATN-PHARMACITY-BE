using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace backend.Controllers
{
    [Route("api/product")] 
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FinalContext db;
        public ProductController(FinalContext _db)
        {
            db = _db;
        }

        [HttpGet("all")]
        /*public async Task<ActionResult<IEnumerable<Products>>> GetAllProduct(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(new
                {
                    message = "Số trang và kích thước trang phải lớn hơn 0!",
                    status = 400
                });
            }

            if (db.Products == null || db.Categories == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var skip = (pageNumber - 1) * pageSize;
            var totalRecords = await db.Products.CountAsync();

            var _data = from product in db.Products
                        join category in db.Categories on product.IdCategory equals category.Id
                        orderby product.CreateAt descending
                        select new
                        {
                            product.Id,
                            product.Name,
                            product.Price,
                            product.Quantity,
                            product.CreateAt,
                            product.Detail,
                            product.IdUser,
                            product.PathImg,
                            category.Slug,
                            product.Type,
                            product.IdCategory,
                            categoryName = category.Name
                        };

            var pagedData = await _data.Skip(skip).Take(pageSize).ToListAsync();

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
                data = pagedData,
                pagination = new
                {
                    currentPage = pageNumber,
                    pageSize,
                    totalRecords,
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                }
            });
        }*/

        public async Task<ActionResult<IEnumerable<Products>>> GetAllProduct()
        {
            if (db.Products == null || db.Categories == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from product in db.Products
                        join category in db.Categories on product.IdCategory equals category.Id
                        orderby product.CreateAt descending
                        select new
                        {
                            product.Id,
                            product.Name,
                            product.Price,
                            product.Quantity,
                            product.CreateAt,
                            product.Detail,
                            product.IdUser,
                            product.PathImg,
                            category.Slug,
                            product.Type,
                            product.IdCategory,
                            categoryName = category.Name
                        };

            var allData = await _data.ToListAsync();

            if (!allData.Any())
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
                data = allData
            });
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProduct(Guid id)
        {
            if (db.Products == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(_data == null)
            {
                return Ok(new
                {
                    message = "Lấy dữ liệu thất bại!",
                    status = 400
                });
            }
            var category = db.Categories.Find(_data.IdCategory);
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data,
                category
            });
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddProduct([FromBody] Products product)
        {
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = product
            });
        }


        [HttpPut("edit")]

        public async Task<ActionResult> Edit([FromBody] Products product)
        {
            var _product = await db.Products.FindAsync(product.Id);
            if (_product == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.Products.FirstOrDefaultAsync(x => x.Id == _product.Id)).CurrentValues.SetValues(product);
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
            if (db.Products == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _product = await db.Products.FindAsync(id);
            if (_product == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.Products.Remove(_product);
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
