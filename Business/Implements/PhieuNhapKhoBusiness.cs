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
    public class PhieuNhapKhoBusiness : INhapKhoBusiness
    {
        QLWebDBEntities dbContext;
        private readonly PhieuNhapReponsitory _phieuNhapRepo;
        private readonly ChiTietPhieuNhapReponsitory _chiTietPhieuNhapRepo;
        private readonly NhanVienReponsitory _nhanVienRepo;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private readonly NhaCungCapReponsitory _nhaCungCapRepo;
        private NhanVienBusiness _nhanVienBus;
        private HangHoaBusiness _hangHoaBus;
        private BaoCaoTonKhoBusiness _baoCaoTonKhoBus;

        public PhieuNhapKhoBusiness()
        {
            dbContext = new QLWebDBEntities();
            _phieuNhapRepo = new PhieuNhapReponsitory(dbContext);
            _nhanVienRepo = new NhanVienReponsitory(dbContext);
            _hangHoaRepo = new HangHoaReponsitory(dbContext);
            _nhaCungCapRepo = new NhaCungCapReponsitory(dbContext);
            _chiTietPhieuNhapRepo = new ChiTietPhieuNhapReponsitory(dbContext);
            _nhanVienBus = new NhanVienBusiness();
            _hangHoaBus = new HangHoaBusiness();
            _baoCaoTonKhoBus = new BaoCaoTonKhoBusiness();
        }

        public IEnumerable<ThongTinHoatDongViewModel> ThongTinHoatDong()
        {
            IQueryable<PhieuNhap> danhSachPhieuNhapKho = _phieuNhapRepo.GetAll();
            List<ThongTinHoatDongViewModel> all = new List<ThongTinHoatDongViewModel>();

            all = (from phieunhapkho in danhSachPhieuNhapKho
                   join nhanvien in _nhanVienRepo.GetAll()
                   on phieunhapkho.MaNhanVien equals nhanvien.MaNhanVien
                   orderby phieunhapkho.NgayChinhSua descending
                   select new
                   {
                       SoPhieuNhapKho = phieunhapkho.SoPhieuNhap,
                       NgayChinhSua = phieunhapkho.NgayChinhSua,
                       TenNhanVien = nhanvien.TenNhanvien,
                       TrangThai = phieunhapkho.TrangThai,
                   }).AsEnumerable().Select(x => new ThongTinHoatDongViewModel()
                   {
                       soPhieuNhapKho = x.SoPhieuNhapKho,
                       ngayChinhSuaNhapKho = x.NgayChinhSua,
                       tenNhanVienNhapKho = x.TenNhanVien,
                       trangThaiNhapKho = x.TrangThai,
                   }).Take(2).ToList();
            return all;
        }

        public IList<PhieuNhapViewModel> SearchDanhSachPhieuNhapKho(string key, string trangThai, DateTime tungay, DateTime denngay, string userName)
        {
            IQueryable<PhieuNhap> danhSachPhieuNhap = _phieuNhapRepo.GetAll();
            IQueryable<NhanVien> danhSachNhanVien = _nhanVienRepo.GetAll();
            IQueryable<NhaCungCap> danhSachNhaCungCap = _nhaCungCapRepo.GetAll();
            List<PhieuNhapViewModel> all = new List<PhieuNhapViewModel>();
            List<PhieuNhapViewModel> allForManager = new List<PhieuNhapViewModel>();

            //Nếu là thủ kho
            if (_nhanVienBus.layMaChucVu(userName) == 5)
            {
                if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
                {
                    allForManager = (from phieunhap in danhSachPhieuNhap
                                     join nhanvien in danhSachNhanVien
                                     on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                                     join nhacungcap in danhSachNhaCungCap
                                     on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                                     where (nhanvien.UserName.Equals(userName) && phieunhap.NgayNhap >= tungay.Date && phieunhap.NgayNhap <= denngay.Date)
                                     select new
                                     {
                                         SoPhieuNhap = phieunhap.SoPhieuNhap,
                                         NgayNhap = phieunhap.NgayNhap,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TenNhaCungCap = nhacungcap.TenNhaCungCap,
                                         TongTien = phieunhap.TongTien,
                                         TrangThai = phieunhap.TrangThai,
                                         GhiChu = phieunhap.Ghichu,

                                     }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                                     {
                                         soPhieuNhap = x.SoPhieuNhap,
                                         ngayNhap = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tenNhaCungCap = x.TenNhaCungCap,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         ghiChu = x.GhiChu,
                                     }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(key))
                {
                    all = (from phieunhap in danhSachPhieuNhap
                           join nhanvien in danhSachNhanVien
                           on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                           join nhacungcap in danhSachNhaCungCap
                           on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                           where (nhanvien.UserName.Equals(userName) && (
                                     phieunhap.SoPhieuNhap.ToString().Equals(key)))
                           select new
                           {
                               SoPhieuNhap = phieunhap.SoPhieuNhap,
                               NgayNhap = phieunhap.NgayNhap,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TenNhaCungCap = nhacungcap.TenNhaCungCap,
                               TongTien = phieunhap.TongTien,
                               TrangThai = phieunhap.TrangThai,
                               GhiChu = phieunhap.Ghichu,

                           }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                           {
                               soPhieuNhap = x.SoPhieuNhap,
                               ngayNhap = x.NgayNhap,
                               tenNhanVien = x.TenNhanVien,
                               tenNhaCungCap = x.TenNhaCungCap,
                               tongTien = x.TongTien,
                               trangThai = x.TrangThai,
                               ghiChu = x.GhiChu,
                           }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return all;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    all = (from phieunhap in danhSachPhieuNhap
                           join nhanvien in danhSachNhanVien
                           on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                           join nhacungcap in danhSachNhaCungCap
                           on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                           where (nhanvien.UserName.Equals(userName) && phieunhap.TrangThai.ToString().Equals(trangThai))
                           select new
                           {
                               SoPhieuNhap = phieunhap.SoPhieuNhap,
                               NgayNhap = phieunhap.NgayNhap,
                               TenNhanVien = nhanvien.TenNhanvien,
                               TenNhaCungCap = nhacungcap.TenNhaCungCap,
                               TongTien = phieunhap.TongTien,
                               TrangThai = phieunhap.TrangThai,
                               GhiChu = phieunhap.Ghichu,

                           }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                           {
                               soPhieuNhap = x.SoPhieuNhap,
                               ngayNhap = x.NgayNhap,
                               tenNhanVien = x.TenNhanVien,
                               tenNhaCungCap = x.TenNhaCungCap,
                               tongTien = x.TongTien,
                               trangThai = x.TrangThai,
                               ghiChu = x.GhiChu,
                           }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return all;

                }

                all = (from phieunhap in danhSachPhieuNhap
                       join nhanvien in danhSachNhanVien
                       on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                       join nhacungcap in danhSachNhaCungCap
                       on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                       where (nhanvien.UserName.Equals(userName))
                       select new
                       {
                           SoPhieuNhap = phieunhap.SoPhieuNhap,
                           NgayNhap = phieunhap.NgayNhap,
                           TenNhanVien = nhanvien.TenNhanvien,
                           TenNhaCungCap = nhacungcap.TenNhaCungCap,
                           TongTien = phieunhap.TongTien,
                           TrangThai = phieunhap.TrangThai,
                           GhiChu = phieunhap.Ghichu,

                       }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                       {
                           soPhieuNhap = x.SoPhieuNhap,
                           ngayNhap = x.NgayNhap,
                           tenNhanVien = x.TenNhanVien,
                           tenNhaCungCap = x.TenNhaCungCap,
                           tongTien = x.TongTien,
                           trangThai = x.TrangThai,
                           ghiChu = x.GhiChu,
                       }).OrderByDescending(x => x.soPhieuNhap).ToList();

                return all;

            }
            else
            {
                if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
                {
                    allForManager = (from phieunhap in danhSachPhieuNhap
                                     join nhanvien in danhSachNhanVien
                                     on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                                     join nhacungcap in danhSachNhaCungCap
                                     on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                                     where (phieunhap.NgayNhap >= tungay.Date && phieunhap.NgayNhap <= denngay.Date)
                                     select new
                                     {
                                         SoPhieuNhap = phieunhap.SoPhieuNhap,
                                         NgayNhap = phieunhap.NgayNhap,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TenNhaCungCap = nhacungcap.TenNhaCungCap,
                                         TongTien = phieunhap.TongTien,
                                         TrangThai = phieunhap.TrangThai,
                                         GhiChu = phieunhap.Ghichu,

                                     }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                                     {
                                         soPhieuNhap = x.SoPhieuNhap,
                                         ngayNhap = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tenNhaCungCap = x.TenNhaCungCap,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         ghiChu = x.GhiChu,
                                     }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(key))
                {
                    allForManager = (from phieunhap in danhSachPhieuNhap
                                     join nhanvien in danhSachNhanVien
                                     on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                                     join nhacungcap in danhSachNhaCungCap
                                     on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                                     where (phieunhap.SoPhieuNhap.ToString().Equals(key))
                                     select new
                                     {
                                         SoPhieuNhap = phieunhap.SoPhieuNhap,
                                         NgayNhap = phieunhap.NgayNhap,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TenNhaCungCap = nhacungcap.TenNhaCungCap,
                                         TongTien = phieunhap.TongTien,
                                         TrangThai = phieunhap.TrangThai,
                                         GhiChu = phieunhap.Ghichu,

                                     }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                                     {
                                         soPhieuNhap = x.SoPhieuNhap,
                                         ngayNhap = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tenNhaCungCap = x.TenNhaCungCap,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         ghiChu = x.GhiChu,
                                     }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return allForManager;

                }
                if (!string.IsNullOrEmpty(trangThai))
                {
                    allForManager = (from phieunhap in danhSachPhieuNhap
                                     join nhanvien in danhSachNhanVien
                                     on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                                     join nhacungcap in danhSachNhaCungCap
                                     on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                                     where phieunhap.TrangThai.ToString().Equals(trangThai)
                                     select new
                                     {
                                         SoPhieuNhap = phieunhap.SoPhieuNhap,
                                         NgayNhap = phieunhap.NgayNhap,
                                         TenNhanVien = nhanvien.TenNhanvien,
                                         TenNhaCungCap = nhacungcap.TenNhaCungCap,
                                         TongTien = phieunhap.TongTien,
                                         TrangThai = phieunhap.TrangThai,
                                         GhiChu = phieunhap.Ghichu,

                                     }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                                     {
                                         soPhieuNhap = x.SoPhieuNhap,
                                         ngayNhap = x.NgayNhap,
                                         tenNhanVien = x.TenNhanVien,
                                         tenNhaCungCap = x.TenNhaCungCap,
                                         tongTien = x.TongTien,
                                         trangThai = x.TrangThai,
                                         ghiChu = x.GhiChu,
                                     }).OrderByDescending(x => x.soPhieuNhap).ToList();

                    return allForManager;
                }

                allForManager = (from phieunhap in danhSachPhieuNhap
                                 join nhanvien in danhSachNhanVien
                                 on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                                 join nhacungcap in danhSachNhaCungCap
                                 on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                                 select new
                                 {
                                     SoPhieuNhap = phieunhap.SoPhieuNhap,
                                     NgayNhap = phieunhap.NgayNhap,
                                     TenNhanVien = nhanvien.TenNhanvien,
                                     TenNhaCungCap = nhacungcap.TenNhaCungCap,
                                     TongTien = phieunhap.TongTien,
                                     TrangThai = phieunhap.TrangThai,
                                     GhiChu = phieunhap.Ghichu,

                                 }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                                 {
                                     soPhieuNhap = x.SoPhieuNhap,
                                     ngayNhap = x.NgayNhap,
                                     tenNhanVien = x.TenNhanVien,
                                     tenNhaCungCap = x.TenNhaCungCap,
                                     tongTien = x.TongTien,
                                     trangThai = x.TrangThai,
                                     ghiChu = x.GhiChu,
                                 }).OrderByDescending(x => x.soPhieuNhap).ToList();

                return allForManager;

            }
        }

        public int LoadSoPhieuNhap()
        {
            var soPhieuNhap = from phieunhap in _phieuNhapRepo.GetAll()
                                 orderby phieunhap.SoPhieuNhap descending
                                 select phieunhap.SoPhieuNhap;

            int demSoPhieu = _phieuNhapRepo.GetAll().Count();
            if (demSoPhieu == 0)
            {
                return 1;
            }
            return (soPhieuNhap.First() + 1);
        }

        public async Task Create(PhieuNhapViewModel O)
        {
            DateTime today = DateTime.Now;
            PhieuNhap order = new PhieuNhap
            {
                SoPhieuNhap = O.soPhieuNhap,
                NgayNhap = today,
                MaNhanVien = O.maNhanVien,
                MaNhaCungCap = O.maNhaCungCap,
                TongTien = O.tongTien,
                Ghichu = O.ghiChu,
                TrangThai = true,
                NgayChinhSua = today,
            };

            int thang = today.Month;
            int nam = today.Year;

            foreach (var i in O.chiTietPhieuNhap)
            {
                order.ChiTietPhieuNhaps.Add(i);

                _hangHoaBus.CapNhapHangHoaVaoBaoCaoTonKhiTaoPhieuNhap(i.MaHangHoa, i.SoLuong, thang, nam);
                _hangHoaBus.CapNhatHangHoaKhiTaoPhieuNhap(i.MaHangHoa, i.SoLuong, i.GiaNhap);

            }
            await _phieuNhapRepo.InsertAsync(order);
        }

        public IEnumerable<PhieuNhapViewModel> thongTinChiTietPhieuNhapTheoMa(int soPhieuNhap)
        {
            IQueryable<ChiTietPhieuNhap> dsPhieuNhap = _chiTietPhieuNhapRepo.GetAll();

            var all = (from chitietphieunhap in dsPhieuNhap
                       join hanghoa in _hangHoaRepo.GetAll()
                       on chitietphieunhap.MaHangHoa equals hanghoa.MaHangHoa
                       join phieunhap in _phieuNhapRepo.GetAll()
                       on chitietphieunhap.SoPhieuNhap equals phieunhap.SoPhieuNhap
                       select new
                       {
                           SoPhieuNhap = chitietphieunhap.SoPhieuNhap,
                           MaHangHoa = hanghoa.MaHangHoa,
                           DonViTinh = hanghoa.DonViTinh,
                           SoLuongNhap = chitietphieunhap.SoLuong,
                           GiaNhap = chitietphieunhap.GiaNhap,
                           ThanhTien = chitietphieunhap.ThanhTien,
                           TenHangHoa = hanghoa.TenHangHoa,
                       }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                       {
                           soPhieuNhap = x.SoPhieuNhap,
                           maHangHoa = x.MaHangHoa,
                           donViTinh = x.DonViTinh,
                           soLuongNhap = x.SoLuongNhap,
                           giaNhap = x.GiaNhap,
                           thanhTien = x.ThanhTien,
                           tenHangHoa = x.TenHangHoa,
                       }).ToList();

            //Lấy thông tin chi tiết phiếu từ số phiếu nhập kho
            var information = (from i in all
                               where (i.soPhieuNhap == soPhieuNhap)
                               select i).ToList();
            return information.ToList();
        }

        public IEnumerable<PhieuNhapViewModel> thongTinPhieuNhapTheoMa(int soPhieuNhap)
        {
            IQueryable<PhieuNhap> danhSachPhieuNhap = _phieuNhapRepo.GetAll();
            List<PhieuNhapViewModel> all = new List<PhieuNhapViewModel>();

            all = (from phieunhap in danhSachPhieuNhap
                   join nhanvien in _nhanVienRepo.GetAll()
                   on phieunhap.MaNhanVien equals nhanvien.MaNhanVien
                   join nhacungcap in _nhaCungCapRepo.GetAll()
                   on phieunhap.MaNhaCungCap equals nhacungcap.MaNhaCungCap
                   where (phieunhap.SoPhieuNhap.Equals(soPhieuNhap))
                   select new
                   {
                       SoPhieuNhap = phieunhap.SoPhieuNhap,
                       NgayNhap = phieunhap.NgayNhap,
                       TenNhanVien = nhanvien.TenNhanvien,
                       TenNhaCungCap = nhacungcap.TenNhaCungCap,
                       TrangThai = phieunhap.TrangThai,
                       TongTien = phieunhap.TongTien,
                       ChuThich = phieunhap.Ghichu,
                   }).AsEnumerable().Select(x => new PhieuNhapViewModel()
                   {
                       soPhieuNhap = x.SoPhieuNhap,
                       ngayNhap = x.NgayNhap,
                       tenNhanVien = x.TenNhanVien,
                       tenNhaCungCap = x.TenNhaCungCap,
                       trangThai = x.TrangThai,
                       tongTien = x.TongTien,
                       ghiChu = x.ChuThich,
                   }).ToList();
            return all;
        }

        public async Task<object> Find(int ID)
        {
            return await _phieuNhapRepo.GetByIdAsync(ID);
        }

        public async Task HuyPhieuNhap(object editModel)
        {
            try
            {
                PhieuNhap editPhieuNhap = (PhieuNhap)editModel;
                editPhieuNhap.TrangThai = false;
                await _phieuNhapRepo.EditAsync(editPhieuNhap);

                var phieuNhapKho = dbContext.ChiTietPhieuNhapes.Where(x => x.SoPhieuNhap == editPhieuNhap.SoPhieuNhap);

                foreach (var i in phieuNhapKho)
                {
                    _hangHoaBus.CapNhapHangHoaVaoBaoCaoTonKhiHuyPhieuNhap(i.MaHangHoa, i.SoLuong, editPhieuNhap.NgayNhap.Month, editPhieuNhap.NgayNhap.Year);
                    _hangHoaBus.CapNhatHangHoaKhiXoaPhieuNhap(i.SoPhieuNhap, i.MaHangHoa, i.SoLuong, i.GiaNhap);
                }
            }
            catch (Exception)
            {

            }           
        }
        public Object LayThongTinPhieuNhap(int id)
        {
            var producInfor = from phieunhap in dbContext.PhieuNhaps
                              where (phieunhap.SoPhieuNhap == id && phieunhap.TrangThai == true)
                              select new
                              {
                                  phieunhap.TongTien                             
                              };
            return producInfor;
        }

    }
}
