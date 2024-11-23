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

        // API thêm mới phiếu nhập kho
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

        // API thêm mới phiếu xuất kho
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
    }
}
