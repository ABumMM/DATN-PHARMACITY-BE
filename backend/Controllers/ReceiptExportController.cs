using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/receiptexport")]
    [ApiController]
    public class ReceiptExportController : ControllerBase
    {
        private readonly FinalContext db;

        public ReceiptExportController(FinalContext _db)
        {
            db = _db;
        }

        [HttpPost("receipt")]
        public async Task<ActionResult> CreateReceipt(WarehouseReceipts receipt)
        {
            if (db.WarehouseReceipts == null)
            {
                return BadRequest(new { message = "Không thể tạo phiếu nhập kho." });
            }

            db.WarehouseReceipts.Add(receipt);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm phiếu nhập kho thành công!",
                status = 200
            });
        }

        [HttpPost("export")]
        public async Task<ActionResult> CreateExport(WarehouseExports export)
        {
            if (db.WarehouseExports == null)
            {
                return BadRequest(new { message = "Không thể tạo phiếu xuất kho." });
            }

            db.WarehouseExports.Add(export);
            await db.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm phiếu xuất kho thành công!",
                status = 200
            });
        }

        [HttpGet("allreceipt")]
        public async Task<ActionResult> GetAllReceipts()
        {
            if (db.WarehouseReceipts == null)
            {
                return NotFound(new { message = "Không có dữ liệu phiếu nhập kho." });
            }

            var receipts = await db.WarehouseReceipts
                .Include(r => r.ReceiptDetails)
                .ToListAsync();

            return Ok(new
            {
                message = "Lấy danh sách phiếu nhập kho thành công!",
                status = 200,
                data = receipts
            });
        }

        [HttpGet("allexport")]
        public async Task<ActionResult> GetAllExports()
        {
            if (db.WarehouseExports == null)
            {
                return NotFound(new { message = "Không có dữ liệu phiếu xuất kho." });
            }

            var exports = await db.WarehouseExports
                .Include(e => e.ExportDetails)
                .ToListAsync();

            return Ok(new
            {
                message = "Lấy danh sách phiếu xuất kho thành công!",
                status = 200,
                data = exports
            });
        }
    }
}
