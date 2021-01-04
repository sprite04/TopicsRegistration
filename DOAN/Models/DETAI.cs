namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        [DisplayName("Tên đề tài")]
        public string TenDeTai { get; set; }

        [DisplayName("Mục tiêu")]
        public string MucTieu { get; set; }

        [DisplayName("Yêu cầu")]
        public string YeuCau { get; set; }

        [StringLength(2000)]
        [DisplayName("Sản phẩm")]
        public string SanPham { get; set; }

        [StringLength(2000)]
        [DisplayName("Chú thích")]
        public string ChuThich { get; set; }

        [DisplayName("Thời gian bắt đầu bảo vệ")]
        public DateTime? ThoiGianBDBaoVe { get; set; }

        [DisplayName("Thời gian kết thúc bảo vệ")]
        public DateTime? ThoiGianKTBaoVe { get; set; }

        [DisplayName("Chuyên ngành")]
        public int? ChuyenNganh { get; set; }

        [DisplayName("Trạng thái")]
        public int? TrangThai { get; set; }

        [DisplayName("Trưởng nhóm")]
        public int? TruongNhom { get; set; }

        [DisplayName("Cấu hình")]
        public int ? CauHinh { get; set; }

        public bool? IsDelete { get; set; }

        [DisplayName("Được đăng ký khác chuyên ngành")]
        public bool? DuocDKKhacCN { get; set; }

        [StringLength(500)]
        [DisplayName("File source")]
        public string File_source { get; set; }

        [StringLength(500)]
        [DisplayName("File word")]
        public string File_word { get; set; }

        [StringLength(500)]
        [DisplayName("File powerpoint")]
        public string File_powerpoint { get; set; }

        [DisplayName("Giáo viên hướng dẫn")]
        public int? GVHuongDan { get; set; }

        [StringLength(20)]
        [DisplayName("Phòng bảo vệ")]
        public string PhongBaoVe { get; set; }

        public bool? IsDuyet { get; set; }

        [Required]
        [DisplayName("Số lượng sinh viên")]
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
