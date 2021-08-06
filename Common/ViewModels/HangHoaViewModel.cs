using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class HangHoaViewModel
    {
        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public decimal giaBan { get; set; }
        public decimal giamGia { get; set; }
        public int soLuongTon { get; set; }
        public string donViTinh { get; set; }
        public string moTa { get; set; }
        public string thongSoKyThuat { get; set; }
        public string xuatXu { get; set; }
        public int thoiGianBaoHanh { get; set; }
        public string hinhAnh { get; set; }
        public int maLoaiHangHoa { get; set; }
        public bool trangThai { get; set; }
        public string modelName { get; set; }

        public string tenLoaiHangHoa { get; set; }
        public int phanTramLoiNhuan { get; set; }

        public string checkImage { get; set; }
    }
}
