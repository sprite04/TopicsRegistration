namespace WEB.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WEBDbContext : DbContext
    {
        public WEBDbContext()
            : base("name=WEB")
        {
        }

        public virtual DbSet<CAUHINH> CAUHINHs { get; set; }
        public virtual DbSet<CHUCVU> CHUCVUs { get; set; }
        public virtual DbSet<CHUYENNGANH> CHUYENNGANHs { get; set; }
        public virtual DbSet<DETAI> DETAIs { get; set; }
        public virtual DbSet<LOAIDETAI> LOAIDETAIs { get; set; }
        public virtual DbSet<LOP> LOPs { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<NIENKHOA> NIENKHOAs { get; set; }
        public virtual DbSet<QUYEN> QUYENs { get; set; }
        public virtual DbSet<SINHVIEN_DETAI> SINHVIEN_DETAI { get; set; }
        public virtual DbSet<THONGBAO> THONGBAOs { get; set; }
        public virtual DbSet<TRANGTHAI> TRANGTHAIs { get; set; }
        public virtual DbSet<USERTYPE> USERTYPEs { get; set; }
        public virtual DbSet<USERTYPE_QUYEN> USERTYPE_QUYEN { get; set; }
        public virtual DbSet<XINVAONHOM> XINVAONHOMs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CAUHINH>()
                .HasMany(e => e.DETAIs)
                .WithOptional(e => e.CAUHINH1)
                .HasForeignKey(e => e.CauHinh);

            modelBuilder.Entity<CHUCVU>()
                .HasMany(e => e.NGUOIDUNGs)
                .WithOptional(e => e.CHUCVU1)
                .HasForeignKey(e => e.ChucVu);

            modelBuilder.Entity<CHUYENNGANH>()
                .HasMany(e => e.DETAIs)
                .WithOptional(e => e.CHUYENNGANH1)
                .HasForeignKey(e => e.ChuyenNganh);

            modelBuilder.Entity<CHUYENNGANH>()
                .HasMany(e => e.NGUOIDUNGs)
                .WithOptional(e => e.CHUYENNGANH1)
                .HasForeignKey(e => e.ChuyenNganh);

            modelBuilder.Entity<DETAI>()
                .Property(e => e.File_source)
                .IsUnicode(false);

            modelBuilder.Entity<DETAI>()
                .Property(e => e.File_word)
                .IsUnicode(false);

            modelBuilder.Entity<DETAI>()
                .Property(e => e.File_powerpoint)
                .IsUnicode(false);

            modelBuilder.Entity<DETAI>()
                .Property(e => e.PhongBaoVe)
                .IsUnicode(false);

            modelBuilder.Entity<DETAI>()
                .HasMany(e => e.SINHVIEN_DETAI)
                .WithRequired(e => e.DETAI1)
                .HasForeignKey(e => e.DeTai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DETAI>()
                .HasMany(e => e.XINVAONHOMs)
                .WithRequired(e => e.DETAI1)
                .HasForeignKey(e => e.DeTai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LOAIDETAI>()
                .HasMany(e => e.CAUHINHs)
                .WithOptional(e => e.LOAIDETAI)
                .HasForeignKey(e => e.LoaiDT);

            modelBuilder.Entity<LOP>()
                .HasMany(e => e.NGUOIDUNGs)
                .WithOptional(e => e.LOP1)
                .HasForeignKey(e => e.Lop);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.CAUHINHs)
                .WithOptional(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.NguoiTao);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.DETAIs)
                .WithOptional(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.GVHuongDan);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.DETAIs1)
                .WithOptional(e => e.NGUOIDUNG1)
                .HasForeignKey(e => e.TruongNhom);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.SINHVIEN_DETAI)
                .WithRequired(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.SinhVien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.THONGBAOs)
                .WithOptional(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.NguoiDang);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.XINVAONHOMs)
                .WithRequired(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.NguoiGui)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NIENKHOA>()
                .HasMany(e => e.CAUHINHs)
                .WithOptional(e => e.NIENKHOA1)
                .HasForeignKey(e => e.NienKhoa);

            modelBuilder.Entity<QUYEN>()
                .Property(e => e.IdQuyen)
                .IsUnicode(false);

            modelBuilder.Entity<QUYEN>()
                .HasMany(e => e.USERTYPE_QUYEN)
                .WithRequired(e => e.QUYEN1)
                .HasForeignKey(e => e.Quyen)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TRANGTHAI>()
                .HasMany(e => e.DETAIs)
                .WithOptional(e => e.TRANGTHAI1)
                .HasForeignKey(e => e.TrangThai);

            modelBuilder.Entity<USERTYPE>()
                .HasMany(e => e.USERTYPE_QUYEN)
                .WithRequired(e => e.USERTYPE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USERTYPE_QUYEN>()
                .Property(e => e.Quyen)
                .IsUnicode(false);
        }
    }
}
