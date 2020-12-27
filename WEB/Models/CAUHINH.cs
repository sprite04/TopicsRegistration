namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
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

        public int? SoLuongSVToiDa { get; set; }

        public bool? Active { get; set; }

        public DateTime? ThoiGianBatDauDK { get; set; }

        public DateTime? ThoiGianKetThucDK { get; set; }

        public int? LoaiDT { get; set; }

        public int? NienKhoa { get; set; }

        public int? HocKy { get; set; }

        public int? NamHocBatDauHocKy { get; set; }

        public int? NamHocKetThucHocKy { get; set; }

        public DateTime? ThoiGianGVBatDauDK { get; set; }

        public DateTime? ThoiGianGVKetThucDK { get; set; }

        public DateTime? ThoiGianSVBatDauNopBC { get; set; }

        public DateTime? ThoiGianSVKetThucNopBC { get; set; }

        public DateTime? DateUpdate { get; set; }

        public DateTime? ThoiGianBatDauDuyet { get; set; }

        public DateTime? ThoiGianKetThucDuyet { get; set; }

        public int? NguoiTao { get; set; }

        public virtual LOAIDETAI LOAIDETAI { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        public virtual NIENKHOA NIENKHOA1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAI> DETAIs { get; set; }
    }
}
