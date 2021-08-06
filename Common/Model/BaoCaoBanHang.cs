namespace Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoBanHang")]
    public partial class BaoCaoBanHang
    {
        [Key]
        public int MaBaoCaoBanHang { get; set; }

        public DateTime NgayBan { get; set; }

        public int SoDonHang { get; set; }

        public decimal TongTien { get; set; }

        
    }
}
