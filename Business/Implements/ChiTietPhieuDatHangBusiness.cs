using Common.Model;
using Common.ViewModels;
using Data.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class ChiTietPhieuDatHangBusiness
    {
        QLWebDBEntities dbContext;
        private readonly ChiTietPhieuDatHangReponsitory _chiTietPhieuDatHangRepo;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private readonly LoaiHangHoaReponsitory _loaiHangHoaRepo;

        public ChiTietPhieuDatHangBusiness()
        {
            dbContext = new QLWebDBEntities();
            _chiTietPhieuDatHangRepo = new ChiTietPhieuDatHangReponsitory(dbContext);
            _hangHoaRepo = new HangHoaReponsitory(dbContext);
            _loaiHangHoaRepo = new LoaiHangHoaReponsitory(dbContext);
        }

        public bool Insert(ChiTietPhieuDatHang detail)
        {
            try
            {
                dbContext.ChiTietPhieuDatHanges.Add(detail);
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }

        public IList<ChiTietPhieuDatHangViewModel> danhSachPhieuDatHangTheoMa(int soPhieuDatHang)
        {
            IQueryable<ChiTietPhieuDatHang> dsChiTietPhieuDatHang = _chiTietPhieuDatHangRepo.GetAll();
            List<ChiTietPhieuDatHangViewModel> all = new List<ChiTietPhieuDatHangViewModel>();


            all = (from chitietphieudathang in dsChiTietPhieuDatHang
                   join hanghoa in _hangHoaRepo.GetAll()
                   on chitietphieudathang.MaHangHoa equals hanghoa.MaHangHoa
                   select new
                   {
                       SoPhieuDatHang = chitietphieudathang.SoPhieuDatHang,
                       MaHangHoa = chitietphieudathang.MaHangHoa,
                       SoLuong = chitietphieudathang.SoLuong,
                       Gia = chitietphieudathang.Gia,
                       ThanhTien = chitietphieudathang.ThanhTien,
                       tenHangHoa = hanghoa.TenHangHoa,
                   }).AsEnumerable().Select(x => new ChiTietPhieuDatHangViewModel()
                   {
                       soPhieuDatHang = x.SoPhieuDatHang,
                       maHangHoa = x.MaHangHoa,
                       soLuong = x.SoLuong,
                       gia = x.Gia,
                       thanhTien = x.ThanhTien,
                       tenHangHoa = x.tenHangHoa,
                   }).ToList();

            var information = (from i in all
                               where (i.soPhieuDatHang == soPhieuDatHang)
                               select i).ToList();

            return information.ToList();
        }
    }
}
