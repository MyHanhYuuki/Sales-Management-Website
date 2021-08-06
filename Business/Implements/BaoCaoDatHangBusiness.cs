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
    public class BaoCaoDatHangBusiness
    {
        QLWebDBEntities dbContext;
        private readonly PhieuDatHangReponsitory _phieuDatHangRepo;
        private readonly NhanVienReponsitory _nhanVienRepo;
        private readonly HangHoaReponsitory _hangHoaRepo;

        private NhanVienBusiness _nhanVienBus;

        public BaoCaoDatHangBusiness()
        {
            dbContext = new QLWebDBEntities();
            _phieuDatHangRepo = new PhieuDatHangReponsitory(dbContext);
            _nhanVienRepo = new NhanVienReponsitory(dbContext);
            _hangHoaRepo = new HangHoaReponsitory(dbContext);
            _nhanVienBus = new NhanVienBusiness();
        }

        public IList<BaoCaoDatHangViewModel> ListView(string nhanVienCode, DateTime dateFrom, DateTime dateTo)
        {
            IQueryable<PhieuDatHang> danhSachPhieuDatHang = _phieuDatHangRepo.GetAll();
            List<BaoCaoDatHangViewModel> allForManager = new List<BaoCaoDatHangViewModel>();

            if ((!(dateFrom == default(DateTime))) && (!(dateTo == default(DateTime))))
            {
                allForManager = (from phieuDatHang in danhSachPhieuDatHang
                                 //join nhanVien in _nhanVienRepo.GetAll()
                                 //on phieuDatHang.MaNhanVien equals nhanVien.MaNhanVien
                                 where phieuDatHang.NgayDat >= dateFrom.Date && phieuDatHang.NgayDat <= dateTo.Date
                                 group phieuDatHang by phieuDatHang.NgayDat into pgroup
                                 select new
                                 {
                                     NgayDat = pgroup.Key,
                                     SoDonHang = pgroup.Count(),
                                     TongTien = pgroup.Sum(phieuDatHang => phieuDatHang.TongTien)
                                 }).AsEnumerable().Select(x => new BaoCaoDatHangViewModel()
                                 {
                                     ngayDat = x.NgayDat,
                                     soDonHang = x.SoDonHang,
                                     tongTien = x.TongTien
                                 }).OrderBy(x => x.ngayDat).ToList();
                return allForManager;
            }
            else
            {
                allForManager = (from phieuDatHang in danhSachPhieuDatHang
                                 //join nhanVien in _nhanVienRepo.GetAll()
                                 //on phieuDatHang.MaNhanVien equals nhanVien.MaNhanVien
                                 where phieuDatHang.NgayDat.Month == DateTime.Now.Month && phieuDatHang.NgayDat.Year == DateTime.Now.Year
                                 group phieuDatHang by phieuDatHang.NgayDat into pgroup
                                 select new
                                 {
                                     NgayDat = pgroup.Key,
                                     SoDonHang = pgroup.Count(),
                                     TongTien = pgroup.Sum(phieuDatHang => phieuDatHang.TongTien)
                                 }).AsEnumerable().Select(x => new BaoCaoDatHangViewModel()
                                 {
                                     ngayDat = x.NgayDat,
                                     soDonHang = x.SoDonHang,
                                     tongTien = x.TongTien
                                 }).OrderBy(x => x.ngayDat).ToList();
                return allForManager;
            }
        }
    }
}
