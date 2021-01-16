namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("XINVAONHOM")]
    public partial class XINVAONHOM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NguoiGui { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeTai { get; set; }

        public DateTime ThoiGian { get; set; }

        public virtual DETAI DETAI1 { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
    }
}
