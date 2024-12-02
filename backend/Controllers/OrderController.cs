using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly FinalContext db;
        public OrderController(FinalContext _db)
        {
            db = _db;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetAllOrder()
        {
            if (db.Users == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from order in db.Orders
                        join user in db.Users on order.IdUser equals user.Id
                        orderby order.CreateAt descending
                        select new
                        {
                            order.Id,
                            order.IdUser,
                            order.Status,
                            user.Name,
                            order.Total,
                            order.CreateAt,
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrder(Guid id)
        {
            if (db.Orders == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Orders.Where(x => x.Id == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] Orders order)
        {
            var _order = await db.Orders.FindAsync(order.Id);
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.Users.FirstOrDefaultAsync(x => x.Id == _order.Id)).CurrentValues.SetValues(order);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddOrder([FromBody] Orders order)
        {
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
            var _data = await db.Orders.Where(x => x.Status == null).Where(x => x.IdUser == order.IdUser).FirstOrDefaultAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpDelete("delete")]

        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            if (db.Orders == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _order = await db.Orders.FindAsync(id);
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.Orders.Remove(_order);
                db.SaveChanges();
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

        [HttpGet("getOrderNotPay")]

        public async Task<ActionResult<Orders>> GetOrderNotPayment(Guid idUser)
        {
            decimal amount = 0;
            var _data = await db.Orders.Where(x => x.IdUser == idUser).Where(x => x.Status == null).FirstOrDefaultAsync();
            if (_data == null)
            {
                return Ok(new
                {
                    message = "Không có order nào hết!",
                    status = 400
                });
            }
            var detail = await db.Detailorders.Where(x => x.IdOrder == _data.Id).ToListAsync();
            if (detail.Count > 0)
            {
                foreach (var item in detail)
                {
                    amount += item.Price * item.Quantity;
                }
            }
            return Ok(new
            {
                message = "Đã có order!",
                status = 200,
                data = _data,
                total = amount
            });
        }

        /*[HttpGet("confirm")]
        public async Task<ActionResult> Confirm(Guid idUser, int status, int type, Guid? idPromotion = null)
        {
            // Lấy thông tin đơn hàng
            var _order = await db.Orders
                .Include(o => o.Promotion)
                .SingleOrDefaultAsync(x => x.IdUser == idUser && x.Status == 0);

            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }

            // Tính tổng tiền từ chi tiết đơn hàng
            var amount = await db.Detailorders
                .Where(x => x.IdOrder == _order.Id)
                .SumAsync(item => item.Price * item.Quantity);

            // Xử lý mã khuyến mãi nếu có
            if (idPromotion.HasValue)
            {
                var promotion = await db.Promotions.FindAsync(idPromotion.Value);
                if (promotion == null || !promotion.IsActive || promotion.StartDate > DateTime.Now || promotion.EndDate < DateTime.Now)
                {
                    return BadRequest(new
                    {
                        message = "Mã khuyến mãi không hợp lệ hoặc đã hết hạn!",
                        status = 400
                    });
                }

                if (promotion.Quantity <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Mã khuyến mãi đã hết số lượng sử dụng!",
                        status = 400
                    });
                }

                // Tính tổng tiền sau giảm giá
                amount -= amount * (promotion.DiscountPercentage / 100);
                _order.IdPromotion = promotion.Id;

                // Giảm số lượng mã khuyến mãi
                promotion.Quantity--;
                db.Promotions.Update(promotion);
            }

            // Cập nhật thông tin đơn hàng
            _order.CreateAt = DateTime.Now;
            _order.Total = amount;
            _order.Status = status;

            db.Orders.Update(_order);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Xác nhận đơn hàng thành công!",
                status = 200
            });
        }*/

        [HttpGet("confirm")]

        public async Task<ActionResult> Confirm(Guid idUser, int status, int type)
        {
            var _order = await db.Orders.Where(x => x.IdUser == idUser).Where(x => x.Status == null).FirstOrDefaultAsync();
            decimal amount = 0;
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            var detail = await db.Detailorders.Where(x => x.IdOrder == _order.Id).ToListAsync();
            if (detail.Count > 0)
            {
                foreach (var item in detail)
                {
                    amount += item.Price * item.Quantity;
                }
            }
            _order.CreateAt = DateTime.Now;
            _order.Total = amount;
            _order.Status = status;
            db.Entry(await db.Orders.FirstOrDefaultAsync(x => x.Id == _order.Id)).CurrentValues.SetValues(_order);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }

        [HttpGet("getAllOrder")]
        public async Task<ActionResult<IEnumerable<Orders>>> GetAllOrderByIdUser(Guid idUser)
        {
            if (db.Users == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Orders.Where(x => x.IdUser == idUser).Where(x => x.Total != 0).OrderByDescending(x => x.CreateAt).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }

        [HttpGet("confirmOrder")]
        public async Task<ActionResult> ConfirmOrder(Guid idOrder, int status)
        {
            var _order = await db.Orders.FindAsync(idOrder);
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            _order.Status = status;
            db.Entry(await db.Orders.FirstOrDefaultAsync(x => x.Id == idOrder)).CurrentValues.SetValues(_order);
            var listProduct = await db.Detailorders.Where(x => x.IdOrder == idOrder).ToListAsync();
            foreach (var item in listProduct)
            {
                var product = await db.Products.FindAsync(item.IdProduct);
                product.Quantity -= item.Quantity;
                db.Entry(await db.Products.FirstOrDefaultAsync(x => x.Id == product.Id)).CurrentValues.SetValues(product);
            }
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
    }
}



