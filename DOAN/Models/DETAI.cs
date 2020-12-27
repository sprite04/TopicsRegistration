namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DETAI")]
    public partial class DETAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DETAI()
        {
            SINHVIEN_DETAI = new HashSet<SINHVIEN_DETAI>();
            XINVAONHOMs = new HashSet<XINVAONHOM>();
        }

        [Key]
        public int IdDeTai { get; set; }

        [StringLength(2000)]
        [Required]
        public string TenDeTai { get; set; }

        public string MucTieu { get; set; }

        public string YeuCau { get; set; }

        [StringLength(2000)]
        public string SanPham { get; set; }

        [StringLength(2000)]
        public string ChuThich { get; set; }

        public DateTime? ThoiGianBDBaoVe { get; set; }

        public DateTime? ThoiGianKTBaoVe { get; set; }

        public int? ChuyenNganh { get; set; }

        public int? TrangThai { get; set; }

        public int? TruongNhom { get; set; }

        public int? CauHinh { get; set; }

        public bool? IsDelete { get; set; }

        public bool? DuocDKKhacCN { get; set; }

        [StringLength(500)]
        public string File_source { get; set; }

        [StringLength(500)]
        public string File_word { get; set; }

        [StringLength(500)]
        public string File_powerpoint { get; set; }

        public int? GVHuongDan { get; set; }

        [StringLength(20)]
        public string PhongBaoVe { get; set; }

        public bool? IsDuyet { get; set; }

        [Required]
        public int? SoLuongSV { get; set; }

        public virtual CAUHINH CAUHINH1 { get; set; }

        public virtual CHUYENNGANH CHUYENNGANH1 { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        public virtual TRANGTHAI TRANGTHAI1 { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SINHVIEN_DETAI> SINHVIEN_DETAI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<XINVAONHOM> XINVAONHOMs { get; set; }
    }
}
