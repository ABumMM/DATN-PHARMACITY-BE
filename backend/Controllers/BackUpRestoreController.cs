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
        private readonly string _backupPath = @"D:\OneDrive\Máy tính\DATN\pharmacy\BackUpRestore";

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
                string filePath = Path.Combine(_backupPath, fileName);

                // Kiểm tra xem file sao lưu có tồn tại không
                if (!System.IO.File.Exists(filePath))
                    return NotFound(new { Message = "File sao lưu không tồn tại!" });

                string connectionString = _configuration.GetConnectionString("dbContext");
                string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

                // Chuyển sang cơ sở dữ liệu master trước khi phục hồi
                string sqlSwitchToMaster = "USE master;";

                // Câu lệnh phục hồi cơ sở dữ liệu
                string sqlRestoreCommand = @$"
            ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            RESTORE DATABASE [{databaseName}]
            FROM DISK = '{filePath}'
            WITH REPLACE, RECOVERY;";  // Phục hồi và áp dụng RECOVERY

                // Kết nối và thực thi lệnh SQL
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Thực thi lệnh chuyển sang cơ sở dữ liệu master và phục hồi
                    using (var sqlCommand = new SqlCommand(sqlSwitchToMaster + sqlRestoreCommand, connection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                return Ok(new { Message = "Phục hồi thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Phục hồi thất bại!", Error = ex.Message });
            }
        }



        private void ExecuteSqlCommand(string command, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
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
