namespace WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USERTYPE_QUYEN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUT { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Quyen { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        public virtual QUYEN QUYEN1 { get; set; }

        public virtual USERTYPE USERTYPE { get; set; }
    }
}
