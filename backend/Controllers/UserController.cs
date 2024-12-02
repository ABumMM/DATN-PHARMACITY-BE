using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FinalContext db;
        private readonly IConfiguration _config;

        public UserController(FinalContext _db, IConfiguration cf)
        {
            db = _db;
            _config = cf;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUser()
        {
            if (db.ApplicationUsers == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from x in db.ApplicationUsers
                        join role in db.ApplicationRoles on x.IdRole equals role.Id
                        select new
                        {
                            x.Id,
                            x.Name,
                            x.Email,
                            x.Password,
                            x.Phone,
                            x.Address,
                            x.CreateAt,
                            x.IdRole,
                            x.PathImg,
                            nameRole = role.Name,
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUser(Guid id)
        {
            if (db.ApplicationUsers == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.ApplicationUsers.Where(x => x.Id == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AddUser([FromBody] Users user)
        {
            var _user = await db.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (_user != null)
            {
                return Ok(new
                {
                    message = "Email đã tồn tại!",
                    status = 400
                });
            }
            var role = await db.ApplicationRoles.Where(x => x.Name.Equals("Guest")).FirstOrDefaultAsync();
            if (user.IdRole == null)
            {
                user.IdRole = role.Id;
            }
            await db.ApplicationUsers.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = user
            });
        }
        /*
                [HttpPost("register")]
                public async Task<ActionResult> AddUser([FromBody] Users user)
                {
                    var _user = await db.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
                    if (_user != null)
                    {
                        return Ok(new
                        {
                            message = "Email đã tồn tại!",
                            status = 400
                        });
                    }

                    // Mã hóa mật khẩu
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    var role = await db.ApplicationRoles.Where(x => x.Name.Equals("Guest")).FirstOrDefaultAsync();
                    if (user.IdRole == null)
                    {
                        user.IdRole = role.Id;
                    }
                    await db.ApplicationUsers.AddAsync(user);
                    await db.SaveChangesAsync();
                    return Ok(new
                    {
                        message = "Tạo thành công!",
                        status = 200,
                        data = user
                    });
                }*/


        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] Users user)
        {
            var _user = await db.ApplicationUsers.FindAsync(user.Id);
            if (_user == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == _user.Id)).CurrentValues.SetValues(user);
            var __user = (from nv in db.ApplicationUsers
                          where nv.Id == user.Id
                          select new
                          {
                              nv.Id,
                              nv.Password,
                              nv.Email,
                              nv.IdRole,
                              nv.PathImg,
                              nv.Address,
                              nv.Name,
                              nv.CreateAt,
                              nv.Phone,
                              role = db.ApplicationRoles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
                          }).ToList();
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200,
                data = __user
            });
        }


        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            if (db.ApplicationUsers == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _user = await db.ApplicationUsers.FindAsync(id);
            if (_user == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.ApplicationUsers.Remove(_user);
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

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login user)
        {
            var _user = (from nv in db.ApplicationUsers
                         where nv.Email == user.email
                         select new
                         {
                             nv.Id,
                             nv.Password,
                             nv.Email,
                             nv.PathImg,
                             nv.IdRole,
                             nv.Address,
                             nv.Name,
                             nv.CreateAt,
                             nv.Phone,
                             role = db.ApplicationRoles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
                         }).ToList();
            if (_user.Count == 0)
            {
                return Ok(new
                {
                    message = "Tài khoản không tồn tại",
                    status = 404
                });
            }
            if (user.password != _user[0].Password)
            {
                return Ok(new
                {
                    message = "Sai mật khẩu",
                    status = 400
                });
            }
            return Ok(new
            {
                message = "Thành công",
                status = 200,
                data = _user,
            });
        }

       /* [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login user)
        {
            var _user = (from nv in db.ApplicationUsers
                         where nv.Email == user.email
                         select new
                         {
                             nv.Id,
                             nv.Password,
                             nv.Email,
                             nv.PathImg,
                             nv.IdRole,
                             nv.Address,
                             nv.Name,
                             nv.CreateAt,
                             nv.Phone,
                             role = db.ApplicationRoles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
                         }).FirstOrDefault();

            if (_user == null)
            {
                return Ok(new
                {
                    message = "Tài khoản không tồn tại",
                    status = 404
                });
            }

            // Kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(user.password, _user.Password))
            {
                return Ok(new
                {
                    message = "Sai mật khẩu",
                    status = 400
                });
            }

            return Ok(new
            {
                message = "Thành công",
                status = 200,
                data = _user,
            });
        }*/


        [HttpGet("info")]
        public ActionResult GetDataFromToken(string token)
        {
            if (token == "undefined")
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 400
                });
            }
            string _token = token.Split(' ')[1];
            if (_token == null)
            {
                return Ok(new
                {
                    message = "Token không đúng!",
                    status = 400
                });
            }
            var handle = new JwtSecurityTokenHandler();
            string email = Regex.Match(JsonSerializer.Serialize(handle.ReadJwtToken(_token)), "emailaddress\",\"Value\":\"(.*?)\",").Groups[1].Value;
            var sinhvien = db.ApplicationUsers.Where(x => x.Email == email).FirstOrDefault();
            if (sinhvien == null)
            {
                return Ok(new
                {
                    message = "Người dùng không tồn tại!",
                    status = 404
                });
            }
            var role = db.ApplicationRoles.Find(sinhvien.IdRole);
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = sinhvien,
                role = role.Name
            });
        }

        /*[HttpPost("changepass")]
        public ActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            var user = db.ApplicationUsers.Find(changePassword.idUser);
            if (user == null)
            {
                return Ok(new
                {
                    message = "Người dùng không tồn tại!",
                    status = 200
                });
            }
            if (changePassword.oldPassword != user.Password)
            {
                return Ok(new
                {
                    message = "Mật khẩu cũ không đúng!",
                    status = 400
                });
            }
            db.Entry(db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id)).CurrentValues.SetValues(user);
            db.SaveChanges();
            return Ok(new
            {
                message = "Thay đổi mật khẩu thành công!",
                status = 200
            });
        }*/

        [HttpPost("changepass")]
        public ActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            var user = db.ApplicationUsers.Find(changePassword.idUser);
            if (user == null)
            {
                return Ok(new
                {
                    message = "Người dùng không tồn tại!",
                    status = 404
                });
            }

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(changePassword.oldPassword, user.Password))
            {
                return Ok(new
                {
                    message = "Mật khẩu cũ không đúng!",
                    status = 400
                });
            }

            // Mã hóa mật khẩu mới
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.newPassword);
            db.Entry(db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id)).CurrentValues.SetValues(user);
            db.SaveChanges();
            return Ok(new
            {
                message = "Thay đổi mật khẩu thành công!",
                status = 200
            });
        }

    }

    public class Login
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class ChangePassword
    {
        public Guid idUser { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
