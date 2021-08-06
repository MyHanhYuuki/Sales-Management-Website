namespace Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoPhieuChi")]
    public partial class BaoCaoPhieuChi
    {
       

        public DateTime NgayChi { get; set; }
        public int SoPhieuChi { get; set; }
       

        public decimal TongTien { get; set; }

      
    }
}
