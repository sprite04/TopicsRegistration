namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAUHINH")]
    public partial class CAUHINH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CAUHINH()
        {
            DETAIs = new HashSet<DETAI>();
        }

        [Key]
        public int IdCauHinh { get; set; }

        [DisplayName("Số lượng sinh viên tối đa")]
        public int? SoLuongSVToiDa { get; set; }

        public bool? Active { get; set; }

        [DisplayName("Thời gian sinh viên bắt đầu đăng ký")]
        public DateTime? ThoiGianBatDauDK { get; set; }

        [DisplayName("Thời gian sinh viên kết thúc đăng ký")]
        public DateTime? ThoiGianKetThucDK { get; set; }

        [DisplayName("Loại đề tài")]
        public int? LoaiDT { get; set; }

        [DisplayName("Niên khoá")]
        public int? NienKhoa { get; set; }

        [DisplayName("Học kỳ")]
        public int? HocKy { get; set; }

        [DisplayName("Năm học bắt đầu học kỳ")]
        public int? NamHocBatDauHocKy { get; set; }

        [DisplayName("Năm học kết thúc học kỳ")]
        public int? NamHocKetThucHocKy { get; set; }

        [DisplayName("Thời gian giáo viên bắt đầu đăng ký")]
        public DateTime? ThoiGianGVBatDauDK { get; set; }

        [DisplayName("Thời gian giáo viên kết thúc đăng ký")]
        public DateTime? ThoiGianGVKetThucDK { get; set; }

        [DisplayName("Thời gian sinh viên bắt đầu nộp báo cáo")]
        public DateTime? ThoiGianSVBatDauNopBC { get; set; }

        [DisplayName("Thời gian sinh viên kết thúc nộp báo cáo")]
        public DateTime? ThoiGianSVKetThucNopBC { get; set; }

        public DateTime? DateUpdate { get; set; }

        [DisplayName("Thời gian bắt đầu duyệt")]
        public DateTime? ThoiGianBatDauDuyet { get; set; }

        [DisplayName("Thời gian kết thúc duyệt")]
        public DateTime? ThoiGianKetThucDuyet { get; set; }

        public int? NguoiTao { get; set; }

        [StringLength(300)]
        public string folderDriveID { get; set; }

        public virtual LOAIDETAI LOAIDETAI { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        public virtual NIENKHOA NIENKHOA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAI> DETAIs { get; set; }
    }
}
