using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using Common.Functions;
using Data.Functions;
using Data.Implements;
using System.Data.Entity.Core.Objects;
using Business.Interfaces;
using System.Web.Mvc;
using Common.ViewModels;
using PagedList;
using System.Web.WebPages.Html;

namespace Business.Implements
{
    public class PhieuBaoHanhBusiness : IPhieuBaoHanhBusiness
    {
        private QLWebDBEntities _dbContext;
        private readonly PhieuBaoHanhReponsitory _phieuBaoHanhRepo;
        private readonly ChiTietPhieuBaoHanhReponsitory _chiTietPhieuBaoHanhRepo;
        private readonly NhanVienReponsitory _nhanVienRepo;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private NhanVienBusiness _nhanVienBus;
        private HangHoaBusiness _hangHoaBus;

        public PhieuBaoHanhBusiness()
        {
            _dbContext = new QLWebDBEntities();
            _phieuBaoHanhRepo = new PhieuBaoHanhReponsitory(_dbContext);
            _chiTietPhieuBaoHanhRepo = new ChiTietPhieuBaoHanhReponsitory(_dbContext);
            _nhanVienRepo = new NhanVienReponsitory(_dbContext);
            _hangHoaRepo = new HangHoaReponsitory(_dbContext);
            _nhanVienBus = new NhanVienBusiness();
            _hangHoaBus = new HangHoaBusiness();
        }

        public IList<PhieuBaoHanhViewModel> SearchDanhSachPhieuBaoHanh(string key, string trangThai, string userName)
        {
            IQueryable<PhieuBaoHanh> danhSachPhieuBaoHanh = _phieuBaoHanhRepo.GetAll();
            IQueryable<NhanVien> danhSachNhanVien = _nhanVienRepo.GetAll();
            List<PhieuBaoHanhViewModel> all = new List<PhieuBaoHanhViewModel>();
            List<PhieuBaoHanhViewModel> allForManager = new List<PhieuBaoHanhViewModel>();

            //Nếu là nhân viên kỹ thuật
            if (_nhanVienBus.layMaChucVu(userName) == 6)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    all = (from phieubaohanh in danhSachPhieuBaoHanh
                           join nhanvien in danhSachNhanVien
                           on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                           where (nhanvien.UserName.Equals(userName) && (
                                     phieubaohanh.SoPhieuBaoHanh.ToString().Equals(key)
                                     || phieubaohanh.TenKhachHang.ToString().Equals(key)
                                     || phieubaohanh.SoDienThoai.ToString().Equals(key)
                                       ))
                           select new
                           {
                               SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                               NgayLap = phieubaohanh.NgayLap,
                               NgayGiao = phieubaohanh.NgayGiao,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TenKhachHang = phieubaohanh.TenKhachHang,
                               SoDienThoai = phieubaohanh.SoDienThoai,
                               GhiChu = phieubaohanh.GhiChu,
                               DaGiao = phieubaohanh.DaGiao,
                               TrangThai = phieubaohanh.TrangThai,
                           }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                           {
                               soPhieuBaoHanh = x.SoPhieuBaoHanh,
                               ngayLap = x.NgayLap,
                               ngayGiao = x.NgayGiao,
                               tenNhanVien = x.TenNhanVien,
                               tenKhachHang = x.TenKhachHang,
                               soDienThoai = x.SoDienThoai,
                               ghiChu = x.GhiChu,
                               daGiao = x.DaGiao,
                               trangThai = x.TrangThai,
                           }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                    return all;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    all = (from phieubaohanh in danhSachPhieuBaoHanh
                           join nhanvien in danhSachNhanVien
                           on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                           where (nhanvien.UserName.Equals(userName) && (
                                     phieubaohanh.TrangThai.ToString().Equals(trangThai)
                                       ))
                           select new
                           {
                               SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                               NgayLap = phieubaohanh.NgayLap,
                               NgayGiao = phieubaohanh.NgayGiao,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TenKhachHang = phieubaohanh.TenKhachHang,
                               SoDienThoai = phieubaohanh.SoDienThoai,
                               GhiChu = phieubaohanh.GhiChu,
                               DaGiao = phieubaohanh.DaGiao,
                               TrangThai = phieubaohanh.TrangThai,
                           }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                           {
                               soPhieuBaoHanh = x.SoPhieuBaoHanh,
                               ngayLap = x.NgayLap,
                               ngayGiao = x.NgayGiao,
                               tenNhanVien = x.TenNhanVien,
                               tenKhachHang = x.TenKhachHang,
                               soDienThoai = x.SoDienThoai,
                               ghiChu = x.GhiChu,
                               daGiao = x.DaGiao,
                               trangThai = x.TrangThai,
                           }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                    return all;

                }

                all = (from phieubaohanh in danhSachPhieuBaoHanh
                       join nhanvien in danhSachNhanVien
                       on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                       where (nhanvien.UserName.Equals(userName))
                       select new
                       {
                           SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                           NgayLap = phieubaohanh.NgayLap,
                           NgayGiao = phieubaohanh.NgayGiao,
                           TenNhanVien = nhanvien.TenNhanvien,
                           TenKhachHang = phieubaohanh.TenKhachHang,
                           SoDienThoai = phieubaohanh.SoDienThoai,
                           GhiChu = phieubaohanh.GhiChu,
                           DaGiao = phieubaohanh.DaGiao,
                           TrangThai = phieubaohanh.TrangThai,
                       }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                       {
                           soPhieuBaoHanh = x.SoPhieuBaoHanh,
                           ngayLap = x.NgayLap,
                           ngayGiao = x.NgayGiao,
                           tenNhanVien = x.TenNhanVien,
                           tenKhachHang = x.TenKhachHang,
                           soDienThoai = x.SoDienThoai,
                           ghiChu = x.GhiChu,
                           daGiao = x.DaGiao,
                           trangThai = x.TrangThai,
                       }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                return all;

            }
            else
            {
                if (!string.IsNullOrEmpty(key))
                {
                    allForManager = (from phieubaohanh in danhSachPhieuBaoHanh
                           join nhanvien in danhSachNhanVien
                           on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                           where (phieubaohanh.SoPhieuBaoHanh.ToString().Equals(key)
                                     || phieubaohanh.TenKhachHang.ToString().Equals(key)
                                     || phieubaohanh.SoDienThoai.ToString().Equals(key))
                           select new
                           {
                               SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                               NgayLap = phieubaohanh.NgayLap,
                               NgayGiao = phieubaohanh.NgayGiao,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TenKhachHang = phieubaohanh.TenKhachHang,
                               SoDienThoai = phieubaohanh.SoDienThoai,
                               GhiChu = phieubaohanh.GhiChu,
                               DaGiao = phieubaohanh.DaGiao,
                               TrangThai = phieubaohanh.TrangThai,
                           }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                           {
                               soPhieuBaoHanh = x.SoPhieuBaoHanh,
                               ngayLap = x.NgayLap,
                               ngayGiao = x.NgayGiao,
                               tenNhanVien = x.TenNhanVien,
                               tenKhachHang = x.TenKhachHang,
                               soDienThoai = x.SoDienThoai,
                               ghiChu = x.GhiChu,
                               daGiao = x.DaGiao,
                               trangThai = x.TrangThai,
                           }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    allForManager = (from phieubaohanh in danhSachPhieuBaoHanh
                                     join nhanvien in danhSachNhanVien
                                     on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                                     where (phieubaohanh.TrangThai.ToString().Equals(trangThai))
                                     select new
                                     {
                                         SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                                         NgayLap = phieubaohanh.NgayLap,
                                         NgayGiao = phieubaohanh.NgayGiao,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TenKhachHang = phieubaohanh.TenKhachHang,
                                         SoDienThoai = phieubaohanh.SoDienThoai,
                                         GhiChu = phieubaohanh.GhiChu,
                                         DaGiao = phieubaohanh.DaGiao,
                                         TrangThai = phieubaohanh.TrangThai,
                                     }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                                     {
                                         soPhieuBaoHanh = x.SoPhieuBaoHanh,
                                         ngayLap = x.NgayLap,
                                         ngayGiao = x.NgayGiao,
                                         tenNhanVien = x.TenNhanVien,
                                         tenKhachHang = x.TenKhachHang,
                                         soDienThoai = x.SoDienThoai,
                                         ghiChu = x.GhiChu,
                                         daGiao = x.DaGiao,
                                         trangThai = x.TrangThai,
                                     }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                    return allForManager;

                }

                allForManager = (from phieubaohanh in danhSachPhieuBaoHanh
                                 join nhanvien in danhSachNhanVien
                                 on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                                 select new
                                 {
                                     SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                                     NgayLap = phieubaohanh.NgayLap,
                                     NgayGiao = phieubaohanh.NgayGiao,
                                     TenNhanVien = nhanvien.TenNhanvien,
                                     TenKhachHang = phieubaohanh.TenKhachHang,
                                     SoDienThoai = phieubaohanh.SoDienThoai,
                                     GhiChu = phieubaohanh.GhiChu,
                                     DaGiao = phieubaohanh.DaGiao,
                                     TrangThai = phieubaohanh.TrangThai,
                                 }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                                 {
                                     soPhieuBaoHanh = x.SoPhieuBaoHanh,
                                     ngayLap = x.NgayLap,
                                     ngayGiao = x.NgayGiao,
                                     tenNhanVien = x.TenNhanVien,
                                     tenKhachHang = x.TenKhachHang,
                                     soDienThoai = x.SoDienThoai,
                                     ghiChu = x.GhiChu,
                                     daGiao = x.DaGiao,
                                     trangThai = x.TrangThai,
                                 }).OrderByDescending(x => x.soPhieuBaoHanh).ToList();

                return allForManager;

            }
        }

        public int LoadSoPhieuBaoHanh()
        {
            var soPhieuBaoHanh = from phieubaohanh in _phieuBaoHanhRepo.GetAll()
                                 orderby phieubaohanh.SoPhieuBaoHanh descending
                                 select phieubaohanh.SoPhieuBaoHanh;

            int demSoPhieu = _phieuBaoHanhRepo.GetAll().Count();
            if (demSoPhieu == 0)
            {
                return 1;
            }
            return (soPhieuBaoHanh.First() + 1);
        }

        public async Task Create(PhieuBaoHanhViewModel O)
        {
            PhieuBaoHanh order = new PhieuBaoHanh
            {
                SoPhieuBaoHanh = O.soPhieuBaoHanh,
                NgayLap = DateTime.Now,
                NgayGiao = O.ngayGiao,
                MaNhanVien = O.maNhanVien,
                TenKhachHang = O.tenKhachHang,
                SoDienThoai = O.soDienThoai,
                GhiChu = O.ghiChu,
                DaGiao = false,
                TrangThai = true,
                NgayChinhSua = DateTime.Now,
            };
            foreach (var i in O.chiTietPhieuBaoHanh)
            {
                order.ChiTietPhieuBaoHanh.Add(i);
            }
            await _phieuBaoHanhRepo.InsertAsync(order);
        }

        public IEnumerable<PhieuBaoHanhViewModel> thongTinChiTietPhieuBaoHanhTheoMa(int soPhieuBaoHanh)
        {
            IQueryable<ChiTietPhieuBaoHanh> dsPhieuBaoHanh = _chiTietPhieuBaoHanhRepo.GetAll();

            var all = (from chitietphieubaohanh in dsPhieuBaoHanh
                       join hanghoa in _hangHoaRepo.GetAll()
                       on chitietphieubaohanh.MaHangHoa equals hanghoa.MaHangHoa
                       join phieubaohanh in _phieuBaoHanhRepo.GetAll()
                       on chitietphieubaohanh.SoPhieuBaoHanh equals phieubaohanh.SoPhieuBaoHanh
                       where (phieubaohanh.SoPhieuBaoHanh == soPhieuBaoHanh)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                           TenHangHoa = hanghoa.TenHangHoa,
                       }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                       {
                           maHangHoa = x.MaHangHoa,
                           tenHangHoa = x.TenHangHoa,
                       }).ToList();

            return all;

        }

        public IEnumerable<PhieuBaoHanhViewModel> thongTinPhieuBaoHanhTheoMa(int soPhieuBaoHanh)
        {
            IQueryable<PhieuBaoHanh> danhSachPhieuBaoHanh = _phieuBaoHanhRepo.GetAll();
            List<PhieuBaoHanhViewModel> all = new List<PhieuBaoHanhViewModel>();

            all = (from phieubaohanh in danhSachPhieuBaoHanh
                   join nhanvien in _nhanVienRepo.GetAll()
                   on phieubaohanh.MaNhanVien equals nhanvien.MaNhanVien
                   where (phieubaohanh.SoPhieuBaoHanh.Equals(soPhieuBaoHanh))
                   select new
                   {
                       SoPhieuBaoHanh = phieubaohanh.SoPhieuBaoHanh,
                       NgayLap = phieubaohanh.NgayLap,
                       NgayGiao = phieubaohanh.NgayGiao,
                       TenNhanVien = nhanvien.TenNhanvien,
                       TenKhachHang = phieubaohanh.TenKhachHang,
                       SoDienThoai = phieubaohanh.SoDienThoai,
                       DaGiao = phieubaohanh.DaGiao,
                       TrangThai = phieubaohanh.TrangThai,
                       GhiChu = phieubaohanh.GhiChu,
                   }).AsEnumerable().Select(x => new PhieuBaoHanhViewModel()
                   {
                       soPhieuBaoHanh = x.SoPhieuBaoHanh,
                       ngayLap = x.NgayLap,
                       ngayGiao = x.NgayGiao,
                       tenNhanVien = x.TenNhanVien,
                       tenKhachHang = x.TenKhachHang,
                       soDienThoai = x.SoDienThoai,
                       daGiao = x.DaGiao,
                       trangThai = x.TrangThai,
                       ghiChu = x.GhiChu,
                   }).ToList();

            return all;

        }

        public async Task<object> Find(int ID)
        {
            return await _phieuBaoHanhRepo.GetByIdAsync(ID);
        }

        public async Task HuyPhieuBaoHanh(object editModel)
        {
            try
            {
                PhieuBaoHanh editPhieuBaoHanh = (PhieuBaoHanh)editModel;
                editPhieuBaoHanh.TrangThai = false;
                await _phieuBaoHanhRepo.EditAsync(editPhieuBaoHanh);
            }
            catch (Exception)
            {

            }
        }

        public async Task XacNhanPhieuBaoHanh(object editModel)
        {
            try
            {
                PhieuBaoHanh editPhieuBaoHanh = (PhieuBaoHanh)editModel;
                editPhieuBaoHanh.DaGiao = true;
                await _phieuBaoHanhRepo.EditAsync(editPhieuBaoHanh);
            }
            catch (Exception)
            {

            }
        }

    }
}
