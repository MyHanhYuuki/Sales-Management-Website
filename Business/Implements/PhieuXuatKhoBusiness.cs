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
    public class PhieuXuatKhoBusiness : IXuatKhoBusiness
    {
        private QLWebDBEntities _dbContext;
        private readonly PhieuXuatKhoReponsitory _phieuXuatKhoRepo;
        private readonly ChiTietPhieuXuatKhoReponsitory _chiTietPhieuXuatKhoRepo;
        private readonly NhanVienReponsitory _nhanVienRepo;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private NhanVienBusiness _nhanVienBus;
        private HangHoaBusiness _hangHoaBus;

        public PhieuXuatKhoBusiness()
        {
            _dbContext = new QLWebDBEntities();
            _phieuXuatKhoRepo = new PhieuXuatKhoReponsitory(_dbContext);
            _nhanVienRepo = new NhanVienReponsitory(_dbContext);
            _hangHoaRepo = new HangHoaReponsitory(_dbContext);
            _chiTietPhieuXuatKhoRepo = new ChiTietPhieuXuatKhoReponsitory(_dbContext);
            _nhanVienBus = new NhanVienBusiness();
            _hangHoaBus = new HangHoaBusiness();
        }

        public IEnumerable<ThongTinHoatDongViewModel> ThongTinHoatDong()
        {
            IQueryable<PhieuXuatKho> danhSachPhieuKiemKho = _phieuXuatKhoRepo.GetAll();
            List<ThongTinHoatDongViewModel> all = new List<ThongTinHoatDongViewModel>();

            all = (from phieuxuatkho in danhSachPhieuKiemKho
                   join nhanvien in _nhanVienRepo.GetAll()
                   on phieuxuatkho.MaNhanVien equals nhanvien.MaNhanVien
                   orderby phieuxuatkho.NgayChinhSua descending
                   select new
                   {
                       SoPhieuXuatKho = phieuxuatkho.SoPhieuXuatKho,
                       NgayChinhSua = phieuxuatkho.NgayChinhSua,
                       TenNhanVien = nhanvien.TenNhanvien,
                       TrangThai = phieuxuatkho.TrangThai,
                   }).AsEnumerable().Select(x => new ThongTinHoatDongViewModel()
                   {
                       soPhieuXuatKho = x.SoPhieuXuatKho,
                       ngayChinhSuaXuatKho = x.NgayChinhSua,
                       tenNhanVienXuatKho = x.TenNhanVien,
                       trangThaiXuatKho = x.TrangThai,
                   }).Take(2).ToList();
            return all;
        }

        public IList<PhieuXuatKhoViewModel> SearchDanhSachPhieuXuatKho(string key, string trangThai, DateTime tungay, DateTime denngay, string userName)
        {
            IQueryable<PhieuXuatKho> danhSachPhieuXuatKho = _phieuXuatKhoRepo.GetAll();
            IQueryable<NhanVien> danhSachNhanVien = _nhanVienRepo.GetAll();
            List<PhieuXuatKhoViewModel> all = new List<PhieuXuatKhoViewModel>();
            List<PhieuXuatKhoViewModel> allForManager = new List<PhieuXuatKhoViewModel>();

            //Nếu là thủ kho
            if (_nhanVienBus.layMaChucVu(userName) == 5)
            {
                if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
                {
                    all = (from phieuxuat in danhSachPhieuXuatKho
                           join nhanvien in danhSachNhanVien
                           on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                           where (nhanvien.UserName.Equals(userName) && phieuxuat.NgayXuat >= tungay.Date && phieuxuat.NgayXuat <= denngay.Date)
                           select new
                           {
                               SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                               NgayNhap = phieuxuat.NgayXuat,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TongTien = phieuxuat.TongTien,
                               TrangThai = phieuxuat.TrangThai,
                               LyDoXuat = phieuxuat.LyDoXuat,
                           }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                           {
                               soPhieuXuatKho = x.SoPhieuXuatKho,
                               ngayXuat = x.NgayNhap,
                               tenNhanVien = x.TenNhanVien,
                               tongTien = x.TongTien,
                               trangThai = x.TrangThai,
                               lyDoXuat = x.LyDoXuat,
                           }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return all;

                }
                if (!string.IsNullOrEmpty(key))
                {
                    all = (from phieuxuat in danhSachPhieuXuatKho
                           join nhanvien in danhSachNhanVien
                           on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                           where (nhanvien.UserName.Equals(userName) && (
                                     phieuxuat.SoPhieuXuatKho.ToString().Equals(key)))
                           select new
                           {
                               SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                               NgayNhap = phieuxuat.NgayXuat,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TongTien = phieuxuat.TongTien,
                               TrangThai = phieuxuat.TrangThai,
                               LyDoXuat = phieuxuat.LyDoXuat,

                           }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                           {
                               soPhieuXuatKho = x.SoPhieuXuatKho,
                               ngayXuat = x.NgayNhap,
                               tenNhanVien = x.TenNhanVien,
                               tongTien = x.TongTien,
                               trangThai = x.TrangThai,
                               lyDoXuat = x.LyDoXuat,
                           }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return all;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    all = (from phieuxuat in danhSachPhieuXuatKho
                           join nhanvien in danhSachNhanVien
                           on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                           where (nhanvien.UserName.Equals(userName) && phieuxuat.TrangThai.ToString().Equals(trangThai))
                           select new
                           {
                               SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                               NgayNhap = phieuxuat.NgayXuat,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TongTien = phieuxuat.TongTien,
                               TrangThai = phieuxuat.TrangThai,
                               LyDoXuat = phieuxuat.LyDoXuat,

                           }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                           {
                               soPhieuXuatKho = x.SoPhieuXuatKho,
                               ngayXuat = x.NgayNhap,
                               tenNhanVien = x.TenNhanVien,
                               tongTien = x.TongTien,
                               trangThai = x.TrangThai,
                               lyDoXuat = x.LyDoXuat,
                           }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return all;

                }

                all = (from phieuxuat in danhSachPhieuXuatKho
                       join nhanvien in danhSachNhanVien
                       on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                       where (nhanvien.UserName.Equals(userName))
                       select new
                       {
                           SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                           NgayNhap = phieuxuat.NgayXuat,
                           TenNhanVien = nhanvien.TenNhanvien,
                           TongTien = phieuxuat.TongTien,
                           TrangThai = phieuxuat.TrangThai,
                           LyDoXuat = phieuxuat.LyDoXuat,

                       }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                       {
                           soPhieuXuatKho = x.SoPhieuXuatKho,
                           ngayXuat = x.NgayNhap,
                           tenNhanVien = x.TenNhanVien,
                           tongTien = x.TongTien,
                           trangThai = x.TrangThai,
                           lyDoXuat = x.LyDoXuat,
                       }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                return all;

            }
            else
            {
                if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
                {
                    allForManager = (from phieuxuat in danhSachPhieuXuatKho
                                     join nhanvien in danhSachNhanVien
                                     on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                                     where (phieuxuat.NgayXuat >= tungay.Date && phieuxuat.NgayXuat <= denngay.Date)
                                     select new
                                     {
                                         SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                                         NgayNhap = phieuxuat.NgayXuat,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TongTien = phieuxuat.TongTien,
                                         TrangThai = phieuxuat.TrangThai,
                                         LyDoXuat = phieuxuat.LyDoXuat,

                                     }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                                     {
                                         soPhieuXuatKho = x.SoPhieuXuatKho,
                                         ngayXuat = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         lyDoXuat = x.LyDoXuat,
                                     }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(key))
                {
                    allForManager = (from phieuxuat in danhSachPhieuXuatKho
                                     join nhanvien in danhSachNhanVien
                                     on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                                     where (phieuxuat.SoPhieuXuatKho.ToString().Equals(key))
                                     select new
                                     {
                                         SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                                         NgayNhap = phieuxuat.NgayXuat,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TongTien = phieuxuat.TongTien,
                                         TrangThai = phieuxuat.TrangThai,
                                         LyDoXuat = phieuxuat.LyDoXuat,

                                     }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                                     {
                                         soPhieuXuatKho = x.SoPhieuXuatKho,
                                         ngayXuat = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         lyDoXuat = x.LyDoXuat,
                                     }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    allForManager = (from phieuxuat in danhSachPhieuXuatKho
                                     join nhanvien in danhSachNhanVien
                                     on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                                     where phieuxuat.TrangThai.ToString().Equals(trangThai)
                                     select new
                                     {
                                         SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                                         NgayNhap = phieuxuat.NgayXuat,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TongTien = phieuxuat.TongTien,
                                         TrangThai = phieuxuat.TrangThai,
                                         LyDoXuat = phieuxuat.LyDoXuat,

                                     }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                                     {
                                         soPhieuXuatKho = x.SoPhieuXuatKho,
                                         ngayXuat = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         lyDoXuat = x.LyDoXuat,
                                     }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                    return allForManager;
                }

                allForManager = (from phieuxuat in danhSachPhieuXuatKho
                                 join nhanvien in danhSachNhanVien
                                 on phieuxuat.MaNhanVien equals nhanvien.MaNhanVien
                                 select new
                                 {
                                     SoPhieuXuatKho = phieuxuat.SoPhieuXuatKho,
                                     NgayNhap = phieuxuat.NgayXuat,
                                     TenNhanVien = nhanvien.TenNhanvien,
                                     TongTien = phieuxuat.TongTien,
                                     TrangThai = phieuxuat.TrangThai,
                                     LyDoXuat = phieuxuat.LyDoXuat,

                                 }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                                 {
                                     soPhieuXuatKho = x.SoPhieuXuatKho,
                                     ngayXuat = x.NgayNhap,
                                     tenNhanVien = x.TenNhanVien,
                                     tongTien = x.TongTien,
                                     trangThai = x.TrangThai,
                                     lyDoXuat = x.LyDoXuat,
                                 }).OrderByDescending(x => x.soPhieuXuatKho).ToList();

                return allForManager;

            }
        }

        public int LoadSoPhieuXuatKho()
        {
            var soPhieuXuatKho = from phieuxuat in _phieuXuatKhoRepo.GetAll()
                                 orderby phieuxuat.SoPhieuXuatKho descending
                                 select phieuxuat.SoPhieuXuatKho;

            int demSoPhieu = _phieuXuatKhoRepo.GetAll().Count();
            if (demSoPhieu == 0)
            {
                return 1;
            }
            return (soPhieuXuatKho.First() + 1);
        }

        public async Task Create(PhieuXuatKhoViewModel O)
        {
            PhieuXuatKho order = new PhieuXuatKho
            {
                SoPhieuXuatKho = O.soPhieuXuatKho,
                NgayXuat = DateTime.Now,
                MaNhanVien = O.maNhanVien,
                TongTien = O.tongTien,
                LyDoXuat = O.lyDoXuat,
                TrangThai = true,
                NgayChinhSua = DateTime.Now,
            };
            foreach (var i in O.chiTietPhieuXuatKho)
            {
                order.ChiTietPhieuXuatKhos.Add(i);
            }
            await _phieuXuatKhoRepo.InsertAsync(order);
        }

        public IEnumerable<PhieuXuatKhoViewModel> thongTinChiTietPhieuXuatKhoTheoMa(int soPhieuXuatKho)
        {
            IQueryable<ChiTietPhieuXuatKho> dsPhieuXuatKho = _chiTietPhieuXuatKhoRepo.GetAll();

            var all = (from chitietphieuxuatkho in dsPhieuXuatKho
                       join hanghoa in _hangHoaRepo.GetAll()
                       on chitietphieuxuatkho.MaHangHoa equals hanghoa.MaHangHoa
                       join phieuxuat in _phieuXuatKhoRepo.GetAll()
                       on chitietphieuxuatkho.SoPhieuXuatKho equals phieuxuat.SoPhieuXuatKho
                       select new
                       {
                           SoPhieuXuatKho = chitietphieuxuatkho.SoPhieuXuatKho,
                           MaHangHoa = hanghoa.MaHangHoa,
                           DonViTinh = hanghoa.DonViTinh,
                           SoLuong = chitietphieuxuatkho.SoLuong,
                           Gia = chitietphieuxuatkho.Gia,
                           ThanhTien = chitietphieuxuatkho.ThanhTien,
                           TenHangHoa = hanghoa.TenHangHoa,
                       }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                       {
                           soPhieuXuatKho = x.SoPhieuXuatKho,
                           maHangHoa = x.MaHangHoa,
                           donViTinh = x.DonViTinh,
                           soLuong = x.SoLuong,
                           gia = x.Gia,
                           thanhTien = x.ThanhTien,
                           tenHangHoa = x.TenHangHoa,
                       }).ToList();

            //Lấy thông tin chi tiết phiếu từ số phiếu nhập kho
            var information = (from i in all
                               where (i.soPhieuXuatKho == soPhieuXuatKho)
                               select i).ToList();
            return information.ToList();

        }

        public IEnumerable<PhieuXuatKhoViewModel> thongTinPhieuXuatKhoTheoMa(int soPhieuXuatKho)
        {
            IQueryable<PhieuXuatKho> danhSachPhieuXuatKho = _phieuXuatKhoRepo.GetAll();
            List<PhieuXuatKhoViewModel> all = new List<PhieuXuatKhoViewModel>();

            all = (from phieuxuatkho in danhSachPhieuXuatKho
                   join nhanvien in _nhanVienRepo.GetAll()
                   on phieuxuatkho.MaNhanVien equals nhanvien.MaNhanVien
                   where (phieuxuatkho.SoPhieuXuatKho.Equals(soPhieuXuatKho))
                   select new
                   {
                       SoPhieuXuatKho = phieuxuatkho.SoPhieuXuatKho,
                       NgayXuat = phieuxuatkho.NgayXuat,
                       TenNhanVien = nhanvien.TenNhanvien,
                       TrangThai = phieuxuatkho.TrangThai,
                       TongTien = phieuxuatkho.TongTien,
                       LyDoXuat = phieuxuatkho.LyDoXuat,
                   }).AsEnumerable().Select(x => new PhieuXuatKhoViewModel()
                   {
                       soPhieuXuatKho = x.SoPhieuXuatKho,
                       ngayXuat = x.NgayXuat,
                       tenNhanVien = x.TenNhanVien,
                       trangThai = x.TrangThai,
                       tongTien = x.TongTien,
                       lyDoXuat = x.LyDoXuat,
                   }).ToList();

            return all;

        }

        public async Task<object> Find(int ID)
        {
            return await _phieuXuatKhoRepo.GetByIdAsync(ID);
        }

        public async Task HuyPhieuXuatKho(object editModel)
        {
            try
            {
                PhieuXuatKho editPhieuXuatKho = (PhieuXuatKho)editModel;
                editPhieuXuatKho.TrangThai = false;
                await _phieuXuatKhoRepo.EditAsync(editPhieuXuatKho);
            }
            catch (Exception)
            {

            }
        }

    }
}
