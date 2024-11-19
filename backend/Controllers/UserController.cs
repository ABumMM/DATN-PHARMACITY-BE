using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        /*[HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUser(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest(new
                {
                    message = "Số trang và kích thước trang phải lớn hơn 0!",
                    status = 400
                });
            }

            if (db.Users == null || db.Roles == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var skip = (pageNumber - 1) * pageSize;

            var _data = from x in db.Users
                        join role in db.Roles on x.IdRole equals role.Id
                        orderby x.CreateAt descending
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
                            nameRole = role.Name
                        };

            var pagedData = await _data.Skip(skip).Take(pageSize).ToListAsync();

            if (!pagedData.Any())
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var totalRecords = await db.Users.CountAsync();

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
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUser()
        {
            if (db.Users == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from x in db.Users
                        join role in db.Roles on x.IdRole equals role.Id
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
            if (db.Users == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.Users.Where(x => x.Id == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }

        /*[HttpPost("register")]
        public async Task<ActionResult> AddUser([FromBody] RegisterRequest request)
        {
            var existingUser = await db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email đã tồn tại!",
                    status = 400
                });
            }

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    message = "Email và mật khẩu không được để trống!",
                    status = 400
                });
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Name.Equals("Guest"));
            var user = new Users
            {
                Name = request.FullName,
                Email = request.Email,
                Password = hashedPassword,
                Phone = request.Phone,
                IdRole = role?.Id,
                CreateAt = DateTime.Now
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var userResponse = new
            {
                FullName = user.Name,
                Email = user.Email,
                Phone = user.Phone
            };

            return Ok(new
            {
                message = "Tạo tài khoản thành công!",
                status = 200,
                data = userResponse
            });
        }*/


        [HttpPost("register")]
        public async Task<ActionResult> AddUser([FromBody] Users user)
        {
            var _user = await db.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (_user != null)
            {
                return Ok(new
                {
                    message = "Email đã tồn tại!",
                    status = 400
                });
            }
            var role = await db.Roles.Where(x => x.Name.Equals("Guest")).FirstOrDefaultAsync();
            if (user.IdRole == null)
            {
                user.IdRole = role.Id;
            }
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = user
            });
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] Users user)
        {
            var _user = await db.Users.FindAsync(user.Id);
            if (_user == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.Users.FirstOrDefaultAsync(x => x.Id == _user.Id)).CurrentValues.SetValues(user);
            var __user = (from nv in db.Users
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
                              role = db.Roles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
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
            if (db.Users == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _user = await db.Users.FindAsync(id);
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
                db.Users.Remove(_user);
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

        /*[HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login user, [FromServices] IConfiguration _config)
        {
            var _user = (from nv in db.Users
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
                             role = db.Roles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
                         }).ToList();

            if (_user.Count == 0)
            {
                return Ok(new
                {
                    message = "Tài khoản không tồn tại",
                    status = 404
                });
            }

            if (!BCrypt.Net.BCrypt.Verify(user.password, _user[0].Password))
            {
                return Ok(new
                {
                    message = "Sai mật khẩu",
                    status = 400
                });
            }

            var token = TokenHelper.Instance.CreateToken(_user[0].Email, _user[0].role, _config);

            var userResponse = new
            {
                FullName = _user[0].Name,
                Email = _user[0].Email,
                Phone = _user[0].Phone,
                Token = token
            };

            return Ok(new
            {
                message = "Đăng nhập thành công",
                status = 200,
                data = userResponse
            });
        }*/
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login user)
        {
            var _user = (from nv in db.Users
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
                             role = db.Roles.Where(x => x.Id == nv.IdRole).FirstOrDefault().Name
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

        /*[HttpGet("info")]
        public ActionResult GetDataFromToken(string token)
        {
            if (string.IsNullOrEmpty(token) || token == "undefined")
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
            var jwtToken = handle.ReadJwtToken(_token);
            var emailClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (emailClaim == null)
            {
                return Ok(new
                {
                    message = "Token không hợp lệ!",
                    status = 400
                });
            }

            var sinhvien = db.Users.Where(x => x.Email == emailClaim).FirstOrDefault();
            if (sinhvien == null)
            {
                return Ok(new
                {
                    message = "Người dùng không tồn tại!",
                    status = 404
                });
            }

            var role = db.Roles.Find(sinhvien.IdRole);
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = sinhvien,
                role = role?.Name ?? "Không có vai trò"
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
            var sinhvien = db.Users.Where(x => x.Email == email).FirstOrDefault();
            if (sinhvien == null)
            {
                return Ok(new
                {
                    message = "Người dùng không tồn tại!",
                    status = 404
                });
            }
            var role = db.Roles.Find(sinhvien.IdRole);
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = sinhvien,
                role = role.Name
            });
        }

        [HttpPost("changepass")]
        public ActionResult ChangePassword([FromBody] ChangePassword changePassword)
        {
            var user = db.Users.Find(changePassword.idUser);
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
            db.Entry(db.Users.FirstOrDefault(x => x.Id == user.Id)).CurrentValues.SetValues(user);
            db.SaveChanges();
            return Ok(new
            {
                message = "Thay đổi mật khẩu thành công!",
                status = 200
            });
        }

    }
/*    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
    }*/

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
