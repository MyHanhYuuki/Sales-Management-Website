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
    public class BaoCaoHangHoaBusiness
    {
        QLWebDBEntities dbContext;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private readonly LoaiHangHoaReponsitory _loaiHangHoaRepo;

        public BaoCaoHangHoaBusiness()
        {
            dbContext = new QLWebDBEntities();
            _hangHoaRepo = new HangHoaReponsitory(dbContext);
            _loaiHangHoaRepo = new LoaiHangHoaReponsitory(dbContext);
        }

        public IList<BaoCaoHangHoaViewModel> ListView(string nhanVienCode, bool trangThai)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            IQueryable<LoaiHangHoa> danhSachLoaiHangHoa = _loaiHangHoaRepo.GetAll();
            List<BaoCaoHangHoaViewModel> allForManager = new List<BaoCaoHangHoaViewModel>();

            allForManager = (from hangHoa in danhSachHangHoa
                             join loaiHangHoa in danhSachLoaiHangHoa
                             on hangHoa.MaLoaiHangHoa equals loaiHangHoa.MaLoaiHangHoa
                             where hangHoa.TrangThai == trangThai
                             select new
                             {
                                 MaHangHoa = hangHoa.MaHangHoa,
                                 TenHangHoa = hangHoa.TenHangHoa,
                                 TenLoaiHangHoa = loaiHangHoa.TenLoaiHangHoa,
                                 GiaBan = hangHoa.GiaBan,
                                 GiamGia = hangHoa.GiamGia,
                                 TrangThai = hangHoa.TrangThai,
                                 SoLuongTon = hangHoa.SoLuongTon,
                                 ModelName = hangHoa.ModelName
                             }).AsEnumerable().Select(x => new BaoCaoHangHoaViewModel()
                             {
                                 maHangHoa = x.MaHangHoa,
                                 tenHangHoa = x.TenHangHoa,
                                 tenLoaiHangHoa = x.TenLoaiHangHoa,
                                 giaBan = x.GiaBan,
                                 giamGia = x.GiamGia,
                                 trangThai = x.TrangThai,
                                 soLuongTon = x.SoLuongTon,
                                 modelName = x.ModelName
                             }).OrderBy(x => x.maHangHoa).ToList();
            return allForManager;
        }
    }
}
