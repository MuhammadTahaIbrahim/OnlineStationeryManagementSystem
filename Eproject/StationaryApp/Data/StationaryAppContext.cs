using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StationaryApp.Models;

namespace StationaryApp.Data
{
    public partial class StationaryAppContext : DbContext
    {
        public StationaryAppContext()
        {
        }

        public StationaryAppContext(DbContextOptions<StationaryAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Addcategory> Addcategories { get; set; } = null!;
        public virtual DbSet<AdminNotification> AdminNotifications { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<ProDetail> ProDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Regretailer> Regretailers { get; set; } = null!;
        public virtual DbSet<Regwholesaler> Regwholesalers { get; set; } = null!;
        public virtual DbSet<RetailerNotification> RetailerNotifications { get; set; } = null!;
        public virtual DbSet<Retailerorder> Retailerorders { get; set; } = null!;
        public virtual DbSet<Retailerorderlist> Retailerorderlists { get; set; } = null!;
        public virtual DbSet<Whorder> Whorders { get; set; } = null!;
        public virtual DbSet<Whsalerorderlist> Whsalerorderlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Initial Catalog=StationaryApp;Persist Security Info=False;User ID=sa;Password=aptech;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Addcategory>(entity =>
            {
                entity.HasKey(e => e.CatId);

                entity.ToTable("addcategory");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.CatDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cat_desc");

                entity.Property(e => e.CatName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cat_name");
            });

            modelBuilder.Entity<AdminNotification>(entity =>
            {
                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WholesalerEmail).HasMaxLength(255);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WholesalerEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("pro_details");

                entity.Property(e => e.Availability)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("availability");

                entity.Property(e => e.CatDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cat_desc");

                entity.Property(e => e.CatName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cat_name");

                entity.Property(e => e.ProDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_desc");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.ProImg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_img");

                entity.Property(e => e.ProName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProPrice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_price");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProId);

                entity.ToTable("product");

                entity.Property(e => e.ProId).HasColumnName("pro_id");

                entity.Property(e => e.Availability)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("availability")
                    .HasDefaultValueSql("('available')");

                entity.Property(e => e.ProDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_desc");

                entity.Property(e => e.ProImg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_img");

                entity.Property(e => e.ProName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProPrice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_price");

                entity.Property(e => e.ProcatidFk).HasColumnName("procatid_fk");

                entity.HasOne(d => d.ProcatidFkNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProcatidFk)
                    .HasConstraintName("FK_product_addcategory");
            });

            modelBuilder.Entity<Regretailer>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__regretai__B9BE370FDFB33372");

                entity.ToTable("regretailer");

                entity.HasIndex(e => e.UserEmail, "UQ__regretai__B0FBA212F83646AC")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPass)
                    .HasMaxLength(255)
                    .HasColumnName("user_pass");

                entity.Property(e => e.UserStatus)
                    .HasMaxLength(50)
                    .HasColumnName("user_status")
                    .HasDefaultValueSql("('Deactive')");
            });

            modelBuilder.Entity<Regwholesaler>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__regwhole__B9BE370FF1952ECD");

                entity.ToTable("regwholesaler");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPass)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_pass");

                entity.Property(e => e.UserStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_status")
                    .HasDefaultValueSql("('Deactive')");
            });

            modelBuilder.Entity<RetailerNotification>(entity =>
            {
                entity.Property(e => e.RetailerEmail).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Retailerorder>(entity =>
            {
                entity.HasKey(e => e.RorderId);

                entity.ToTable("retailerorder");

                entity.Property(e => e.RorderId).HasColumnName("rorder_id");

                entity.Property(e => e.Catename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("catename");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Retaileremail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("retaileremail");

                entity.Property(e => e.RproIdfk).HasColumnName("rpro_idfk");

                entity.Property(e => e.Rproqty).HasColumnName("rproqty");

                entity.Property(e => e.Rstatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rstatus");

                entity.Property(e => e.Rtotalprice)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("rtotalprice");

                entity.HasOne(d => d.RproIdfkNavigation)
                    .WithMany(p => p.Retailerorders)
                    .HasForeignKey(d => d.RproIdfk)
                    .HasConstraintName("FK_retailerorder_product");
            });

            modelBuilder.Entity<Retailerorderlist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("retailerorderlist");

                entity.Property(e => e.Catename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("catename");

                entity.Property(e => e.ProDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_desc");

                entity.Property(e => e.ProImg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_img");

                entity.Property(e => e.ProName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProPrice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_price");

                entity.Property(e => e.Retaileremail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("retaileremail");

                entity.Property(e => e.RorderId).HasColumnName("rorder_id");

                entity.Property(e => e.Rproqty).HasColumnName("rproqty");

                entity.Property(e => e.Rstatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rstatus");

                entity.Property(e => e.Rtotalprice)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("rtotalprice");
            });

            modelBuilder.Entity<Whorder>(entity =>
            {
                entity.HasKey(e => e.WorderId);

                entity.ToTable("whorder");

                entity.Property(e => e.WorderId).HasColumnName("worder_id");

                entity.Property(e => e.Catename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("catename");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Whsaleremail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("whsaleremail");

                entity.Property(e => e.WproIdfk).HasColumnName("wpro_idfk");

                entity.Property(e => e.Wproqty).HasColumnName("wproqty");

                entity.Property(e => e.Wstatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("wstatus");

                entity.Property(e => e.Wtotalprice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("wtotalprice");

                entity.HasOne(d => d.WproIdfkNavigation)
                    .WithMany(p => p.Whorders)
                    .HasForeignKey(d => d.WproIdfk)
                    .HasConstraintName("FK_whorder_product");
            });

            modelBuilder.Entity<Whsalerorderlist>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("whsalerorderlist");

                entity.Property(e => e.Catename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("catename");

                entity.Property(e => e.ProDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_desc");

                entity.Property(e => e.ProImg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_img");

                entity.Property(e => e.ProName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_name");

                entity.Property(e => e.ProPrice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pro_price");

                entity.Property(e => e.Whsaleremail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("whsaleremail");

                entity.Property(e => e.WorderId).HasColumnName("worder_id");

                entity.Property(e => e.Wproqty).HasColumnName("wproqty");

                entity.Property(e => e.Wstatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("wstatus");

                entity.Property(e => e.Wtotalprice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("wtotalprice");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
