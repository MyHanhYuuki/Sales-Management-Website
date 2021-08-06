using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;

namespace Common.ViewModels
{
    public class PhieuNhapViewModel
    {
        public int soPhieuNhap { get; set; }
        public DateTime ngayNhap { get; set; }
        public DateTime ngayChinhSua { get; set; }
        public int maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public int maNhaCungCap { get; set; }
        public string tenNhaCungCap { get; set; }
        public string ghiChu { get; set; }
        public decimal tongTien { get; set; }
        public bool trangThai { get; set; }

        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public string donViTinh { get; set; }
        public int soLuongNhap { get; set; }
        public decimal giaNhap { get; set; }
        public decimal thanhTien { get; set; }

        public List<ChiTietPhieuNhap> chiTietPhieuNhap { get; set; }
    }
}
