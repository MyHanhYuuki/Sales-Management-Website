﻿namespace Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuBanHang")]
    public partial class ChiTietPhieuBanHang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SoPhieuBanHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHangHoa { get; set; }

        public int SoLuong { get; set; }

        public decimal Gia { get; set; }

        public decimal ThanhTien { get; set; }

        //[StringLength(200)]
        //public string NoiDungBaoHanh { get; set; }

        //[StringLength(200)]
        //public string GhiChu { get; set; }
    }
}
