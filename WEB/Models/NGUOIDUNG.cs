namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NGUOIDUNG")]
    public partial class NGUOIDUNG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NGUOIDUNG()
        {
            CAUHINHs = new HashSet<CAUHINH>();
            DETAIs = new HashSet<DETAI>();
            DETAIs1 = new HashSet<DETAI>();
            SINHVIEN_DETAI = new HashSet<SINHVIEN_DETAI>();
            THONGBAOs = new HashSet<THONGBAO>();
            XINVAONHOMs = new HashSet<XINVAONHOM>();
        }

        [Key]
        public int IdUser { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Avatar { get; set; }

        public bool? Block { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RegisterDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastVisitDate { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string HocVi { get; set; }

        public int? ChuyenNganh { get; set; }

        public int? Lop { get; set; }

        public int? ChucVu { get; set; }

        public double? Diem { get; set; }

        public int? TongTC { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgaySinh { get; set; }

        public bool GioiTinh { get; set; }

        public int? IdUT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAUHINH> CAUHINHs { get; set; }

        public virtual CHUCVU CHUCVU1 { get; set; }

        public virtual CHUYENNGANH CHUYENNGANH1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAI> DETAIs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAI> DETAIs1 { get; set; }

        public virtual LOP LOP1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SINHVIEN_DETAI> SINHVIEN_DETAI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THONGBAO> THONGBAOs { get; set; }

        public virtual USERTYPE USERTYPE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<XINVAONHOM> XINVAONHOMs { get; set; }
    }
}
