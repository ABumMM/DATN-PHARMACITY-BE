using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class FinalContext : IdentityDbContext<Users, Roles, Guid>
{
    public FinalContext(DbContextOptions<FinalContext> options) : base(options) { }

    // user Identity
    public virtual DbSet<Users> ApplicationUsers => Set<Users>();
    public virtual DbSet<Roles> ApplicationRoles => Set<Roles>();
    public virtual DbSet<RoleClaims> ApplicationRoleClaims => Set<RoleClaims>();
    public virtual DbSet<UserClaims> ApplicationUserClaims => Set<UserClaims>();
    public virtual DbSet<UserLogins> ApplicationUserLogins => Set<UserLogins>();
    public virtual DbSet<UserTokens> ApplicationUserTokens => Set<UserTokens>();

    // Các entity khác viết dưới này

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Detailorders> Detailorders { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<Promotions> Promotions { get; set; }

    public virtual DbSet<Suppliers> Suppliers { get; set; }

    public virtual DbSet<Warehouses> Warehouses { get; set; }

    public virtual DbSet<WarehouseExportDetails> WarehouseExportDetails { get; set; }

    public virtual DbSet<WarehouseExports> WarehouseExports { get; set; }

    public virtual DbSet<WarehouseReceiptDetails> WarehouseReceiptDetails { get; set; }

    public virtual DbSet<WarehouseReceipts> WarehouseReceipts { get; set; }

    public virtual DbSet<WarehouseProducts> WarehouseProducts { get; set; }
}
