namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THONGBAO")]
    public partial class THONGBAO
    {
        [Key]
        public int IdTB { get; set; }

        [StringLength(200)]
        [DisplayName("Tiêu đề")]
        public string TenTB { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Nội dung")]
        public string NoiDung { get; set; }

        public int? NguoiDang { get; set; }

        public DateTime? NgayDang { get; set; }

        [DisplayName("Số ngày hiển thị")]
        public int? SoNgayHienThi { get; set; }

        public bool? CoTinMoi { get; set; }

        public bool? IsDelete { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
