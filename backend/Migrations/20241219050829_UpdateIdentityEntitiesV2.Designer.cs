﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Models;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(FinalContext))]
    [Migration("20241219050829_UpdateIdentityEntitiesV2")]
    partial class UpdateIdentityEntitiesV2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRoleClaim<Guid>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserClaim<Guid>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserLogin<Guid>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUserToken<Guid>");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("backend.Models.Categories", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("backend.Models.Detailorders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdOrder")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdOrderNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProductNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdOrderNavigationId");

                    b.HasIndex("IdProductNavigationId");

                    b.ToTable("Detailorders");
                });

            modelBuilder.Entity("backend.Models.Orders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdPromotion")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IduserNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PromotionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.Property<decimal?>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IduserNavigationId");

                    b.HasIndex("PromotionId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("backend.Models.Products", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("IdCategory")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdCategoryNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdUserNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdCategoryNavigationId");

                    b.HasIndex("IdUserNavigationId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("backend.Models.Promotions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("backend.Models.Roles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("backend.Models.Suppliers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("backend.Models.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("IdRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdRoleNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("IdRoleNavigationId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("backend.Models.WarehouseExportDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdExport")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdExportNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProductNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdExportNavigationId");

                    b.HasIndex("IdProductNavigationId");

                    b.ToTable("WarehouseExportDetails");
                });

            modelBuilder.Entity("backend.Models.WarehouseExports", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExportDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdWarehouse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdWarehouseNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdWarehouseNavigationId");

                    b.ToTable("WarehouseExports");
                });

            modelBuilder.Entity("backend.Models.WarehouseProducts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProductNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdWarehouse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdWarehouseNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdProductNavigationId");

                    b.HasIndex("IdWarehouseNavigationId");

                    b.ToTable("WarehouseProducts");
                });

            modelBuilder.Entity("backend.Models.WarehouseReceiptDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdProductNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdReceipt")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdReceiptNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdProductNavigationId");

                    b.HasIndex("IdReceiptNavigationId");

                    b.ToTable("WarehouseReceiptDetails");
                });

            modelBuilder.Entity("backend.Models.WarehouseReceipts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdSupplier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdSupplierNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdWarehouse")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IdWarehouseNavigationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReceiptDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdSupplierNavigationId");

                    b.HasIndex("IdWarehouseNavigationId");

                    b.ToTable("WarehouseReceipts");
                });

            modelBuilder.Entity("backend.Models.Warehouses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("backend.Models.RoleClaims", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("RoleClaims");
                });

            modelBuilder.Entity("backend.Models.UserClaims", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("UserClaims");
                });

            modelBuilder.Entity("backend.Models.UserLogins", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("UserLogins");
                });

            modelBuilder.Entity("backend.Models.UserTokens", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("UserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("backend.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("backend.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("backend.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("backend.Models.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("backend.Models.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Models.Detailorders", b =>
                {
                    b.HasOne("backend.Models.Orders", "IdOrderNavigation")
                        .WithMany("Detailorders")
                        .HasForeignKey("IdOrderNavigationId");

                    b.HasOne("backend.Models.Products", "IdProductNavigation")
                        .WithMany("Detailorders")
                        .HasForeignKey("IdProductNavigationId");

                    b.Navigation("IdOrderNavigation");

                    b.Navigation("IdProductNavigation");
                });

            modelBuilder.Entity("backend.Models.Orders", b =>
                {
                    b.HasOne("backend.Models.Users", "IduserNavigation")
                        .WithMany("Orders")
                        .HasForeignKey("IduserNavigationId");

                    b.HasOne("backend.Models.Promotions", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionId");

                    b.Navigation("IduserNavigation");

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("backend.Models.Products", b =>
                {
                    b.HasOne("backend.Models.Categories", "IdCategoryNavigation")
                        .WithMany("Products")
                        .HasForeignKey("IdCategoryNavigationId");

                    b.HasOne("backend.Models.Users", "IdUserNavigation")
                        .WithMany("Products")
                        .HasForeignKey("IdUserNavigationId");

                    b.Navigation("IdCategoryNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("backend.Models.Users", b =>
                {
                    b.HasOne("backend.Models.Roles", "IdRoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdRoleNavigationId");

                    b.Navigation("IdRoleNavigation");
                });

            modelBuilder.Entity("backend.Models.WarehouseExportDetails", b =>
                {
                    b.HasOne("backend.Models.WarehouseExports", "IdExportNavigation")
                        .WithMany("ExportDetails")
                        .HasForeignKey("IdExportNavigationId");

                    b.HasOne("backend.Models.Products", "IdProductNavigation")
                        .WithMany()
                        .HasForeignKey("IdProductNavigationId");

                    b.Navigation("IdExportNavigation");

                    b.Navigation("IdProductNavigation");
                });

            modelBuilder.Entity("backend.Models.WarehouseExports", b =>
                {
                    b.HasOne("backend.Models.Warehouses", "IdWarehouseNavigation")
                        .WithMany()
                        .HasForeignKey("IdWarehouseNavigationId");

                    b.Navigation("IdWarehouseNavigation");
                });

            modelBuilder.Entity("backend.Models.WarehouseProducts", b =>
                {
                    b.HasOne("backend.Models.Products", "IdProductNavigation")
                        .WithMany()
                        .HasForeignKey("IdProductNavigationId");

                    b.HasOne("backend.Models.Warehouses", "IdWarehouseNavigation")
                        .WithMany("WarehouseProducts")
                        .HasForeignKey("IdWarehouseNavigationId");

                    b.Navigation("IdProductNavigation");

                    b.Navigation("IdWarehouseNavigation");
                });

            modelBuilder.Entity("backend.Models.WarehouseReceiptDetails", b =>
                {
                    b.HasOne("backend.Models.Products", "IdProductNavigation")
                        .WithMany()
                        .HasForeignKey("IdProductNavigationId");

                    b.HasOne("backend.Models.WarehouseReceipts", "IdReceiptNavigation")
                        .WithMany("ReceiptDetails")
                        .HasForeignKey("IdReceiptNavigationId");

                    b.Navigation("IdProductNavigation");

                    b.Navigation("IdReceiptNavigation");
                });

            modelBuilder.Entity("backend.Models.WarehouseReceipts", b =>
                {
                    b.HasOne("backend.Models.Suppliers", "IdSupplierNavigation")
                        .WithMany()
                        .HasForeignKey("IdSupplierNavigationId");

                    b.HasOne("backend.Models.Warehouses", "IdWarehouseNavigation")
                        .WithMany()
                        .HasForeignKey("IdWarehouseNavigationId");

                    b.Navigation("IdSupplierNavigation");

                    b.Navigation("IdWarehouseNavigation");
                });

            modelBuilder.Entity("backend.Models.Categories", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("backend.Models.Orders", b =>
                {
                    b.Navigation("Detailorders");
                });

            modelBuilder.Entity("backend.Models.Products", b =>
                {
                    b.Navigation("Detailorders");
                });

            modelBuilder.Entity("backend.Models.Roles", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("backend.Models.Users", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("backend.Models.WarehouseExports", b =>
                {
                    b.Navigation("ExportDetails");
                });

            modelBuilder.Entity("backend.Models.WarehouseReceipts", b =>
                {
                    b.Navigation("ReceiptDetails");
                });

            modelBuilder.Entity("backend.Models.Warehouses", b =>
                {
                    b.Navigation("WarehouseProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
