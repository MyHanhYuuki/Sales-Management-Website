using Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class PhieuBanHangViewModel
    {
        public int soPhieuBanHang { get; set; }
        public DateTime ngayBan { get; set; }
        public DateTime ngayChinhSua { get; set; }
        public string tenNhanVien { get; set; }
        public string ghiChu { get; set; }
        public bool trangThai { get; set; }
        [Required]
        public string tenKhachHang { get; set; }
        [Required]
        public string soDienThoai { get; set; }
        public decimal tongTien { get; set; }
        public List<ChiTietPhieuBanHang> chiTietPhieuBanHang { get; set; }

    }
}
