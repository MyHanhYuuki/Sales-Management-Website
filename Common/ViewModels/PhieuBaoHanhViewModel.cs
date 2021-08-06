using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;

namespace Common.ViewModels
{
    public class PhieuBaoHanhViewModel
    {
        public int soPhieuBaoHanh { get; set; }
        public DateTime ngayLap { get; set; }
        public DateTime ngayGiao { get; set; }
        public int maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public string tenKhachHang { get; set; }
        public string soDienThoai { get; set; }
        public decimal tongTien { get; set; }
        public string ghiChu { get; set; }
        public bool daGiao { get; set; }
        public DateTime ngayChinhSua { get; set; }
        public bool trangThai { get; set; }

        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        //public int soLuong { get; set; }
        //public string noiDungBaoHanh { get; set; }
        //public decimal gia { get; set; }
        //public decimal thanhTien { get; set; }
        //public string ghiChu { get; set; }

        public List<ChiTietPhieuBaoHanh> chiTietPhieuBaoHanh { get; set; }
    }
}
