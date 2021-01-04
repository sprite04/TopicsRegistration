namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_ThongBao
    {
        [Key]
        public int IdTB { get; set; }

        [StringLength(200)]
        public string TenTB { get; set; }

        public int? NguoiDang { get; set; }

        public DateTime? NgayDang { get; set; }

        public int? SoNgayHienThi { get; set; }

        public bool? CoTinMoi { get; set; }

        [Column(TypeName = "ntext")]
        public string NoiDung { get; set; }
    }
}
