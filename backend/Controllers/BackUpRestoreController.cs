using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackUpController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _backupPath = @"D:\Backup";

        public BackUpController(IConfiguration configuration)
        {
            _configuration = configuration;

            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);
        }

        [HttpPost("backup")]
        public IActionResult BackupDatabase()
        {
            try
            {
                string fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string filePath = Path.Combine(_backupPath, fileName);

                string connectionString = _configuration.GetConnectionString("dbContext");
                string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

                string sqlBackupCommand = @$"
                    BACKUP DATABASE [{databaseName}]
                    TO DISK = '{filePath}'
                    WITH FORMAT, INIT, NAME = 'Database Backup';";

                ExecuteSqlCommand(sqlBackupCommand, connectionString);

                return Ok(new { Message = "Backup thành công!", FileName = fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Backup thất bại!", Error = ex.Message });
            }
        }

        [HttpGet("list")]
        public IActionResult GetBackupFiles()
        {
            try
            {
                var files = Directory.GetFiles(_backupPath)
                                     .Where(f => f.EndsWith(".bak"))
                                     .Select(Path.GetFileName)
                                     .ToList();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lấy danh sách file backup thất bại!", Error = ex.Message });
            }
        }

        [HttpPost("restore")]
        public IActionResult RestoreDatabase([FromBody] string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest(new { Message = "Tên file không được để trống." });
                }

                string filePath = Path.Combine(_backupPath, fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { Message = "File backup không tồn tại.", FileName = fileName });
                }

                string connectionString = _configuration.GetConnectionString("dbContext");
                string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

                string sqlRestoreCommand = @$"
            ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            RESTORE DATABASE [{databaseName}]
            FROM DISK = '{filePath}'
            WITH REPLACE;
            ALTER DATABASE [{databaseName}] SET MULTI_USER;";

                ExecuteSqlCommand(sqlRestoreCommand, connectionString);

                return Ok(new { Message = "Khôi phục cơ sở dữ liệu thành công!", FileName = fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Khôi phục cơ sở dữ liệu thất bại!", Error = ex.Message });
            }
        }




        private void ExecuteSqlCommand(string command, string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var sqlCommand = new SqlCommand(command, connection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
