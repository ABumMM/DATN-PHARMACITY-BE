using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Tạo phiếu nhập kho
        [HttpPost("receipt")]
        public async Task<ActionResult> CreateReceipt([FromBody] WarehouseReceipts receipt)
        {
            if (receipt == null || !receipt.ReceiptDetails.Any())
                return BadRequest(new { message = "Thông tin phiếu nhập không hợp lệ." });

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var warehouse = await db.Warehouses.FindAsync(receipt.IdWarehouse);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                var supplier = await db.Suppliers.FindAsync(receipt.IdSupplier);
                if (supplier == null)
                    return NotFound(new { message = "Nhà cung cấp không tồn tại." });

                // Gán Id mới cho WarehouseReceipts
                receipt.Id = Guid.NewGuid();

                db.WarehouseReceipts.Add(receipt);
                await db.SaveChangesAsync(); // Lưu WarehouseReceipts trước để lấy Id

                foreach (var detail in receipt.ReceiptDetails)
                {
                    // Sửa cách đặt tên cho đúng quy ước camelCase
                    var expirationDate = detail.ExpirationDate ?? DateTime.MaxValue;

                    // Thêm chi tiết phiếu nhập vào database
                    db.WarehouseReceiptDetails.Add(new WarehouseReceiptDetails
                    {
                        Id = Guid.NewGuid(),
                        IdReceipt = receipt.Id, // Gán Id của WarehouseReceipts vừa tạo
                        IdProduct = detail.IdProduct, // Sử dụng đúng tên thuộc tính
                        Quantity = detail.Quantity, // Sử dụng đúng tên thuộc tính
                        ExpirationDate = expirationDate // Sử dụng đúng tên thuộc tính
                    });

                    var warehouseProduct = await db.WarehouseProducts
                        .FirstOrDefaultAsync(wp => wp.IdWarehouse == receipt.IdWarehouse && wp.IdProduct == detail.IdProduct);

                    if (warehouseProduct == null)
                    {
                        db.WarehouseProducts.Add(new WarehouseProducts
                        {
                            Id = Guid.NewGuid(),
                            IdWarehouse = receipt.IdWarehouse,
                            IdProduct = detail.IdProduct,
                            Quantity = detail.Quantity,
                            ExpirationDate = expirationDate
                        });
                    }
                    else
                    {
                        if (warehouseProduct.ExpirationDate != expirationDate)
                        {
                            db.WarehouseProducts.Add(new WarehouseProducts
                            {
                                Id = Guid.NewGuid(),
                                IdWarehouse = receipt.IdWarehouse,
                                IdProduct = detail.IdProduct,
                                Quantity = detail.Quantity,
                                ExpirationDate = expirationDate
                            });
                        }
                        else
                        {
                            warehouseProduct.Quantity += detail.Quantity;
                        }
                    }
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Tạo phiếu nhập kho thành công!",
                    status = 200,
                    data = new
                    {
                        receiptId = receipt.Id,
                        warehouseName = warehouse.Name,
                        supplierName = supplier.Name,
                        receiptDetails = receipt.ReceiptDetails
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        [HttpPost("export")]
        public async Task<ActionResult> CreateExport([FromBody] WarehouseExports export)
        {
            if (export == null || !export.ExportDetails.Any())
                return BadRequest(new { message = "Thông tin phiếu xuất không hợp lệ." });

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var warehouse = await db.Warehouses.FindAsync(export.IdWarehouse);
                if (warehouse == null)
                    return NotFound(new { message = "Kho hàng không tồn tại." });

                // Gán Id mới cho WarehouseExports
                export.Id = Guid.NewGuid();
                export.ExportDate = DateTime.Now;

                db.WarehouseExports.Add(export);
                await db.SaveChangesAsync(); // Lưu WarehouseExports trước để lấy Id

                foreach (var detail in export.ExportDetails)
                {
                    var warehouseProduct = await db.WarehouseProducts
                        .Where(wp => wp.IdWarehouse == export.IdWarehouse && wp.IdProduct == detail.IdProduct)
                        .OrderBy(wp => wp.ExpirationDate)
                        .FirstOrDefaultAsync();

                    if (warehouseProduct == null || warehouseProduct.Quantity < detail.Quantity)
                        return BadRequest(new { message = $"Không đủ số lượng sản phẩm (ID: {detail.IdProduct}) trong kho để xuất." });

                    // Thêm chi tiết phiếu xuất vào database
                    db.WarehouseExportDetails.Add(new WarehouseExportDetails
                    {
                        Id = Guid.NewGuid(),
                        IdExport = export.Id,
                        IdProduct = detail.IdProduct,
                        Quantity = detail.Quantity,
                        ExpirationDate = warehouseProduct.ExpirationDate,
                    });

                    await db.SaveChangesAsync(); // Lưu WarehouseExportDetails

                    warehouseProduct.Quantity -= detail.Quantity;

                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Tạo phiếu xuất kho thành công!",
                    status = 200,
                    data = new
                    {
                        exportId = export.Id,
                        warehouseName = warehouse.Name,
                        exportDetails = export.ExportDetails
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }


        // Lấy tất cả phiếu nhập kho
        [HttpGet("receipts/all")]
        public async Task<ActionResult> GetAllReceipts()
        {
            if (db.WarehouseReceipts == null || db.WarehouseReceiptDetails == null || db.Warehouses == null || db.Suppliers == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from receipt in db.WarehouseReceipts
                        join warehouse in db.Warehouses on receipt.IdWarehouse equals warehouse.Id
                        join supplier in db.Suppliers on receipt.IdSupplier equals supplier.Id
                        orderby receipt.ReceiptDate descending
                        select new
                        {
                            receipt.Id,
                            receipt.ReceiptDate,
                            warehouseName = warehouse.Name,
                            supplierName = supplier.Name,
                            receiptDetails = (from detail in db.WarehouseReceiptDetails
                                              where detail.IdReceipt == receipt.Id
                                              join product in db.Products on detail.IdProduct equals product.Id
                                              select new
                                              {
                                                  detail.IdProduct,
                                                  productName = product.Name,
                                                  detail.Quantity,
                                                  detail.ExpirationDate
                                              }).ToList()
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
                message = "Lấy danh sách phiếu nhập kho thành công!",
                status = 200,
                data = allData
            });
        }



        // Lấy tất cả phiếu xuất kho
        [HttpGet("exports/all")]
        public async Task<ActionResult> GetAllExports()
        {
            if (db.WarehouseExports == null || db.WarehouseExportDetails == null || db.Warehouses == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }

            var _data = from export in db.WarehouseExports
                        join warehouse in db.Warehouses on export.IdWarehouse equals warehouse.Id
                        orderby export.ExportDate descending
                        select new
                        {
                            export.Id,
                            export.ExportDate,
                            warehouseName = warehouse.Name,
                            exportDetails = (from detail in db.WarehouseExportDetails
                                             where detail.IdExport == export.Id
                                             join product in db.Products on detail.IdProduct equals product.Id
                                             select new
                                             {
                                                 detail.IdProduct,
                                                 productName = product.Name,
                                                 detail.Quantity,
                                                 detail.ExpirationDate
                                             }).ToList()
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
                message = "Lấy danh sách phiếu xuất kho thành công!",
                status = 200,
                data = allData
            });
        }



    }
}
