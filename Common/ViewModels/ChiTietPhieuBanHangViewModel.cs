﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class ChiTietPhieuBanHangViewModel
    {
        public int soPhieuBanHang { get; set; }
        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public int soLuong { get; set; }
        public decimal gia { get; set; }
        public decimal thanhTien { get; set; }

        public string hinhAnh { get; set; }
    }
}
