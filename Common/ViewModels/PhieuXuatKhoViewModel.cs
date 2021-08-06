using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;

namespace Common.ViewModels
{
    public class PhieuXuatKhoViewModel
    {
        public int soPhieuXuatKho { get; set; }
        public DateTime ngayXuat { get; set; }
        public int maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public string lyDoXuat { get; set; }
        public decimal tongTien { get; set; }
        public DateTime ngayChinhSua { get; set; }
        public bool trangThai { get; set; }

        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public string donViTinh { get; set; }
        public int soLuong { get; set; }
        public int soLuongHienTai { get; set; }
        public decimal gia { get; set; }
        public decimal thanhTien { get; set; }

        public List<ChiTietPhieuXuatKho> chiTietPhieuXuatKho { get; set; }
    }
}