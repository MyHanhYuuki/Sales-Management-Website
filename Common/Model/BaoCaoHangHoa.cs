namespace Common.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoHangHoa")]
    public partial class BaoCaoHangHoa
    {
        [Key]
        public int MaBaoCaoHangHoa { get; set; }

        public int MaHangHoa { get; set; }

        public string TenHangHoa { get; set; }

        public string ModelName { get; set; }

        public string TenLoaiHangHoa { get; set; }

        public decimal GiaBan { get; set; }

        public decimal GiamGia { get; set; }

        public bool TrangThai { get; set; }
        public int SoLuongTon { get; set; }
    }
}
