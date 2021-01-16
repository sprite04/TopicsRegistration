namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NIENKHOA")]
    public partial class NIENKHOA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NIENKHOA()
        {
            CAUHINHs = new HashSet<CAUHINH>();
            LOPs = new HashSet<LOP>();
        }

        [Key]
        public int IdNK { get; set; }

        [StringLength(50)]
        [DisplayName("Niên khoá")]
        public string TenNK { get; set; }

        [DisplayName("Năm bắt đầu")]
        public int? NamBD { get; set; }

        [DisplayName("Năm kết thúc")]
        public int? NamKT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAUHINH> CAUHINHs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOP> LOPs { get; set; }
    }
}
