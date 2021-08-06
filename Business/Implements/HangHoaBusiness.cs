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
    public class HangHoaBusiness : IHangHoaBusiness
    {
        private QLWebDBEntities _dbContext;
        private readonly HangHoaReponsitory _hangHoaRepo;
        private readonly LoaiHangHoaReponsitory _loaiHangHoaRepo;
        private readonly ChiTietPhieuBanHangReponsitory _chiTietPhieuBanHangRepo;
        private readonly ChiTietPhieuDatHangReponsitory _chiTietPhieuDatHangRepo;
        public HangHoaBusiness()
        {
            _dbContext = new QLWebDBEntities();
            _hangHoaRepo = new HangHoaReponsitory(_dbContext);
            _loaiHangHoaRepo = new LoaiHangHoaReponsitory(_dbContext);
            _chiTietPhieuBanHangRepo = new ChiTietPhieuBanHangReponsitory(_dbContext);
            _chiTietPhieuDatHangRepo = new ChiTietPhieuDatHangReponsitory(_dbContext); 
        }

        public IEnumerable<HangHoa> GetAllHangHoa()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       select new
                       {
                           TenHangHoa = hanghoa.TenHangHoa,
                           ModelName = hanghoa.ModelName,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           TenHangHoa = x.TenHangHoa,
                           ModelName = x.ModelName,
                       }).ToList();
            return all;
        }

        public IList<HangHoaViewModel> SearchDanhSachHangHoa(String key, string trangThai)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoaViewModel> all = new List<HangHoaViewModel>();

            all = (from hanghoa in danhSachHangHoa
                   join loaihanghoa in _loaiHangHoaRepo.GetAll()
                   on hanghoa.MaLoaiHangHoa equals loaihanghoa.MaLoaiHangHoa
                   where (hanghoa.TrangThai.ToString().Equals(trangThai)
                        || hanghoa.TenHangHoa.ToString().Contains(key)
                        || hanghoa.ModelName.ToString().Contains(key)
                        || loaihanghoa.TenLoaiHangHoa.ToString().Contains(key))
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       SoLuongTon = hanghoa.SoLuongTon,
                       XuatXu = hanghoa.XuatXu,
                       ThoiGianBaoHanh = hanghoa.ThoiGianBaoHanh,
                       MaLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                       TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       ModelName = hanghoa.ModelName,
                       TrangThai = hanghoa.TrangThai,
                       HinhAnh = hanghoa.HinhAnh,
                   }).AsEnumerable().Select(x => new HangHoaViewModel()
                   {
                       maHangHoa = x.MaHangHoa,
                       tenHangHoa = x.TenHangHoa,
                       giaBan = x.GiaBan,
                       giamGia = x.GiamGia,
                       soLuongTon = x.SoLuongTon,
                       xuatXu = x.XuatXu,
                       thoiGianBaoHanh = x.ThoiGianBaoHanh,
                       maLoaiHangHoa = x.MaLoaiHangHoa,
                       tenLoaiHangHoa = x.TenLoaiHangHoa,
                       modelName = x.ModelName,
                       trangThai = x.TrangThai,
                       hinhAnh = x.HinhAnh,
                   }).ToList();

            return all;

        }

        public IEnumerable<HangHoaViewModel> LoadDanhSachHangHoa()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoaViewModel> all = new List<HangHoaViewModel>();

            all = (from hanghoa in danhSachHangHoa
                   join loaihanghoa in _loaiHangHoaRepo.GetAll()
                   on hanghoa.MaLoaiHangHoa equals loaihanghoa.MaLoaiHangHoa
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       SoLuongTon = hanghoa.SoLuongTon,
                       XuatXu = hanghoa.XuatXu,
                       ThoiGianBaoHanh = hanghoa.ThoiGianBaoHanh,
                       MaLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                       TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       ModelName = hanghoa.ModelName,
                       TrangThai = hanghoa.TrangThai,
                       HinhAnh = hanghoa.HinhAnh,
                   }).AsEnumerable().Select(x => new HangHoaViewModel()
                   {
                       maHangHoa = x.MaHangHoa,
                       tenHangHoa = x.TenHangHoa,
                       giaBan = x.GiaBan,
                       giamGia = x.GiamGia,
                       soLuongTon = x.SoLuongTon,
                       xuatXu = x.XuatXu,
                       thoiGianBaoHanh = x.ThoiGianBaoHanh,
                       maLoaiHangHoa = x.MaLoaiHangHoa,
                       tenLoaiHangHoa = x.TenLoaiHangHoa,
                       modelName = x.ModelName,
                       trangThai = x.TrangThai,
                       hinhAnh = x.HinhAnh,
                   }).ToList();

            return all;

        }

        public IEnumerable<HangHoaViewModel> LoadDanhSachHangHoaTheoMa(int maHangHoa)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();

            var all = (from hanghoa in danhSachHangHoa
                       join loaihanghoa in _loaiHangHoaRepo.GetAll()
                       on hanghoa.MaLoaiHangHoa equals loaihanghoa.MaLoaiHangHoa
                       where (hanghoa.MaHangHoa.Equals(maHangHoa))
                       select new HangHoaViewModel
                       {
                           maHangHoa = hanghoa.MaHangHoa,
                           tenHangHoa = hanghoa.TenHangHoa,
                           giaBan = hanghoa.GiaBan,
                           giamGia = hanghoa.GiamGia,
                           soLuongTon = hanghoa.SoLuongTon,
                           donViTinh = hanghoa.DonViTinh,
                           moTa = hanghoa.MoTa,
                           thongSoKyThuat = hanghoa.ThongSoKyThuat,
                           xuatXu = hanghoa.XuatXu,
                           thoiGianBaoHanh = hanghoa.ThoiGianBaoHanh,
                           maLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                           tenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                           modelName = hanghoa.ModelName,
                           trangThai = hanghoa.TrangThai,
                           hinhAnh = hanghoa.HinhAnh,
                       }).ToList();

            return all;
        }

        public async Task Create(object model)
        {
            var hangHoa = new HangHoa();
            HangHoaViewModel input = (HangHoaViewModel)model;

            hangHoa.TenHangHoa = input.tenHangHoa;
            hangHoa.GiaBan = input.giaBan;
            hangHoa.GiamGia = input.giamGia;
            hangHoa.SoLuongTon = input.soLuongTon;
            hangHoa.DonViTinh = input.donViTinh;
            hangHoa.MoTa = input.moTa;
            hangHoa.ThongSoKyThuat = input.thongSoKyThuat;
            hangHoa.XuatXu = input.xuatXu;
            hangHoa.ThoiGianBaoHanh = input.thoiGianBaoHanh;
            hangHoa.MaLoaiHangHoa = input.maLoaiHangHoa;
            hangHoa.ModelName = input.modelName;
            hangHoa.TrangThai = true;
            hangHoa.HinhAnh = input.hinhAnh;

            await _hangHoaRepo.InsertAsync(hangHoa);
        }

        public async Task<object> Find(int ID)
        {
            return await _hangHoaRepo.GetByIdAsync(ID);
        }

        public async Task Update(object inputModel, object editModel)
        {
            HangHoaViewModel input = (HangHoaViewModel)inputModel;
            HangHoa editHangHoa = (HangHoa)editModel;

            editHangHoa.TenHangHoa = input.tenHangHoa;
            editHangHoa.GiaBan = input.giaBan;
            editHangHoa.GiamGia = input.giamGia;
            editHangHoa.SoLuongTon = input.soLuongTon;
            editHangHoa.DonViTinh = input.donViTinh;
            editHangHoa.MoTa = input.moTa;
            editHangHoa.ThongSoKyThuat = input.thongSoKyThuat;
            editHangHoa.XuatXu = input.xuatXu;
            editHangHoa.ThoiGianBaoHanh = input.thoiGianBaoHanh;
            editHangHoa.ModelName = input.modelName;
            editHangHoa.MaLoaiHangHoa = input.maLoaiHangHoa;
            editHangHoa.TrangThai = input.trangThai;
            editHangHoa.HinhAnh = input.hinhAnh;

            await _hangHoaRepo.EditAsync(editHangHoa);
        }

        public List<Object> LoadSanhSachHangHoaKho()
        {
            var list = (from hanghoa in _dbContext.HangHoas
                        where (hanghoa.TrangThai == true)
                        select new SelectListItem
                        {
                            Text = hanghoa.TenHangHoa,
                            Value = hanghoa.MaHangHoa.ToString(),
                        }).Distinct().ToList();

            return new List<Object>(list);
        }

        public List<string> ListName(string keyword)
        {
            return _dbContext.HangHoas.Where(x => x.TenHangHoa.Contains(keyword)).Select(x => x.TenHangHoa).ToList();
        }

        public Object LayThongTinHangHoa(int maHangHoa)
        {
            var producInfor = from hanghoa in _dbContext.HangHoas
                              where (hanghoa.MaHangHoa == maHangHoa && hanghoa.TrangThai == true)
                              select new
                              {
                                  hanghoa.TenHangHoa,
                                  hanghoa.DonViTinh,
                                  hanghoa.SoLuongTon,
                                  hanghoa.GiaBan,
                                  hanghoa.GiamGia,
                                  hanghoa.HinhAnh,
                                  hanghoa.ModelName
                              };
            return producInfor;
        }

        public bool CapNhatHangHoaKhiTaoPhieuNhap(int maHangHoa, int soLuongNhap, decimal giaNhap)
        {
            try
            {
                var loinhuan = from loaihanghoa in _dbContext.LoaiHangHoas
                               join hanghoa in _hangHoaRepo.GetAll()
                               on loaihanghoa.MaLoaiHangHoa equals hanghoa.MaLoaiHangHoa
                               where hanghoa.MaHangHoa.Equals(maHangHoa)
                               select new
                               {
                                   loaihanghoa.PhanTramLoiNhuan
                               };

                decimal phantramloinhuan = loinhuan.FirstOrDefault().PhanTramLoiNhuan;

                decimal phantram = 1 + phantramloinhuan / 100;

                var result = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);

                if (result != null)
                {
                    decimal giaChuaTinhLoiNhuan = Math.Round(result.GiaBan / phantram);

                    decimal giaBinhQuan = Math.Round((result.SoLuongTon * giaChuaTinhLoiNhuan + soLuongNhap * giaNhap) / (result.SoLuongTon + soLuongNhap));
                    result.GiaBan = Math.Round(giaBinhQuan + giaBinhQuan * phantramloinhuan / 100);
                    result.SoLuongTon += soLuongNhap;
                    _dbContext.SaveChanges();

                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool CapNhatHangHoaKhiXoaPhieuNhap(int soPhieuNhap, int maHangHoa, int soLuongNhap, decimal giaNhap)
        {
            try
            {
                var loinhuan = from loaihanghoa in _dbContext.LoaiHangHoas
                               join hanghoa in _hangHoaRepo.GetAll()
                               on loaihanghoa.MaLoaiHangHoa equals hanghoa.MaLoaiHangHoa
                               where hanghoa.MaHangHoa.Equals(maHangHoa)
                               select new
                               {
                                   loaihanghoa.PhanTramLoiNhuan
                               };
                decimal phantramloinhuan = loinhuan.FirstOrDefault().PhanTramLoiNhuan;

                decimal phantram = 1 + phantramloinhuan / 100;

                var result = _dbContext.ChiTietPhieuNhapes.FirstOrDefault(x => x.SoPhieuNhap == soPhieuNhap && x.MaHangHoa == maHangHoa);

                var MatHangCanUpdate = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);

                if (result != null)
                {
                    int sl = MatHangCanUpdate.SoLuongTon - soLuongNhap;
                    if (sl != 0)
                    {
                        decimal giaChuaTinhLoiNhuan = Math.Round(MatHangCanUpdate.GiaBan / phantram);

                        MatHangCanUpdate.GiaBan = phantram * Math.Round((giaChuaTinhLoiNhuan * (MatHangCanUpdate.SoLuongTon) - giaNhap * soLuongNhap) / (MatHangCanUpdate.SoLuongTon - soLuongNhap));

                        MatHangCanUpdate.SoLuongTon = MatHangCanUpdate.SoLuongTon - soLuongNhap;

                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        MatHangCanUpdate.GiaBan = 0;
                        MatHangCanUpdate.SoLuongTon = 0;
                        _dbContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhatHangHoaKhiTaoPhieuBanHang(int maHangHoa, int soLuongBan)
        {
            try
            {
                var MatHangCanUpdate = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);
                MatHangCanUpdate.SoLuongTon -= soLuongBan;

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhatHangHoaKhiXoaPhieuBanHang(int maHangHoa, int soLuongBan)
        {
            try
            {
                var MatHangCanUpdate = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);
                MatHangCanUpdate.SoLuongTon += soLuongBan;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int LaySoLuongTonCuoiCuaThangTruoc(int maHangHoa, int thang, int nam)
        {
            if(thang == 1)
            {
                var result = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == 12 && x.Nam == (nam - 1));
                if (result != null)
                {
                    return result.SoLuongTonCuoi;
                }
                else return 0;
            }
            
            var result1 = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == (thang - 1) && x.Nam == nam);
            if (result1 != null)
            {
                return result1.SoLuongTonCuoi;
            }
            else return 0;
        }

        public bool CapNhapHangHoaVaoBaoCaoTonKhiTaoPhieuNhap(int maHangHoa, int soLuongNhap, int thang, int nam)
        {
            try
            {
                var result = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == thang && x.Nam == nam);

                if(result != null)
                {
                    result.SoLuongNhap += soLuongNhap;
                    result.SoLuongTonCuoi = result.SoLuongTonDau + result.SoLuongNhap - result.SoLuongXuat;
                    _dbContext.SaveChanges();
                }
                else
                {
                    var hanghoa = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);
                    BaoCaoTonKho baocaotonkho = new BaoCaoTonKho
                    {
                        Thang = thang,
                        Nam = nam,
                        MaHangHoa = maHangHoa,
                        SoLuongTonDau = hanghoa.SoLuongTon,
                        SoLuongNhap = soLuongNhap,
                        SoLuongXuat = 0,
                        SoLuongTonCuoi = hanghoa.SoLuongTon + soLuongNhap - 0
                    };

                    _dbContext.BaoCaoTonKhos.Add(baocaotonkho);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhapHangHoaVaoBaoCaoTonKhiHuyPhieuNhap(int maHangHoa, int soLuongNhap, int thang, int nam)
        {
            try
            {
                var result = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == thang && x.Nam == nam);

                if (result != null)
                {
                    result.SoLuongNhap -= soLuongNhap;
                    result.SoLuongTonCuoi = result.SoLuongTonDau + result.SoLuongNhap - result.SoLuongXuat;
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhapHangHoaVaoBaoCaoTonKhiTaoPhieuBanHang(int maHangHoa, int soLuongXuat, int thang, int nam)
        {
            try
            {
                var result = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == thang && x.Nam == nam);

                if (result != null)
                {
                    result.SoLuongXuat += soLuongXuat;
                    result.SoLuongTonCuoi = result.SoLuongTonDau + result.SoLuongNhap - result.SoLuongXuat;
                    _dbContext.SaveChanges();
                }
                else
                {
                    var hanghoa = _dbContext.HangHoas.FirstOrDefault(x => x.MaHangHoa == maHangHoa);
                    BaoCaoTonKho baocaotonkho = new BaoCaoTonKho
                    {
                        Thang = thang,
                        Nam = nam,
                        MaHangHoa = maHangHoa,
                        SoLuongTonDau = hanghoa.SoLuongTon,
                        SoLuongNhap = 0,
                        SoLuongXuat = soLuongXuat,
                        SoLuongTonCuoi = hanghoa.SoLuongTon + 0 - soLuongXuat
                    };

                    _dbContext.BaoCaoTonKhos.Add(baocaotonkho);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CapNhapHangHoaVaoBaoCaoTonKhiHuyPhieuBanHang(int maHangHoa, int soLuongXuat, int thang, int nam)
        {
            try
            {
                var result = _dbContext.BaoCaoTonKhos.FirstOrDefault(x => x.MaHangHoa == maHangHoa && x.Thang == thang && x.Nam == nam);

                if (result != null)
                {
                    result.SoLuongXuat -= soLuongXuat;
                    result.SoLuongTonCuoi = result.SoLuongTonDau + result.SoLuongNhap - result.SoLuongXuat;
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<HangHoa> DanhSachHangHoaMoiNhat()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoa> all = new List<HangHoa>();

            all = (from hanghoa in danhSachHangHoa
                   where (hanghoa.TrangThai == true) && (hanghoa.GiamGia == 0 && hanghoa.GiaBan > 0)
                   orderby hanghoa.MaHangHoa descending
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       XuatXu = hanghoa.XuatXu,
                   }).AsEnumerable().Select(x => new HangHoa()
                   {
                       MaHangHoa = x.MaHangHoa,
                       TenHangHoa = x.TenHangHoa,
                       HinhAnh = x.HinhAnh,
                       GiaBan = x.GiaBan,
                       GiamGia = x.GiamGia,
                       XuatXu = x.XuatXu,
                   }).Take(8).ToList();
            return all;
        }

        public IEnumerable<HangHoa> LoadHangHoaTheoMa(int maHangHoa)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoa> all = new List<HangHoa>();

            all = (from hanghoa in danhSachHangHoa
                   where (hanghoa.MaHangHoa.Equals(maHangHoa) && hanghoa.TrangThai == true)
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       ThongSoKyThuat = hanghoa.ThongSoKyThuat,
                       SoLuongTon = hanghoa.SoLuongTon,
                       ThoiGianBaoHanh = hanghoa.ThoiGianBaoHanh,
                       Mota = hanghoa.MoTa,
                   }).AsEnumerable().Select(x => new HangHoa()
                   {
                       MaHangHoa = x.MaHangHoa,
                       TenHangHoa = x.TenHangHoa,
                       HinhAnh = x.HinhAnh,
                       GiaBan = x.GiaBan,
                       GiamGia = x.GiamGia,
                       ThongSoKyThuat = x.ThongSoKyThuat,
                       SoLuongTon = x.SoLuongTon,
                       ThoiGianBaoHanh = x.ThoiGianBaoHanh,
                       MoTa = x.Mota,
                   }).ToList();
            return all;
        }

        public string TenLoaiHangHoaTheoMaLoaiHangHoa(int maLoaiHangHoa)
        {
            IQueryable<LoaiHangHoa> loaiHangHoa = _loaiHangHoaRepo.GetAll();
            return loaiHangHoa.FirstOrDefault(x => x.MaLoaiHangHoa.Equals(maLoaiHangHoa)).TenLoaiHangHoa;
        }

        public object TongSanPhamTheoLoaiHang(int maLoaiHangHoa)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       join loaihanghoa in _loaiHangHoaRepo.GetAll()
                       on hanghoa.MaLoaiHangHoa equals loaihanghoa.MaLoaiHangHoa
                       where hanghoa.TrangThai.Equals(true) && hanghoa.MaLoaiHangHoa.Equals(maLoaiHangHoa)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Distinct().Count();
            return all;
        }

        public IList<HangHoaViewModel> DanhSachHangHoaTheoMaLoaiHangHoa(int maLoaiHangHoa)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoaViewModel> all = new List<HangHoaViewModel>();

            all = (from hanghoa in danhSachHangHoa
                   join loaihanghoa in _loaiHangHoaRepo.GetAll()
                   on hanghoa.MaLoaiHangHoa equals loaihanghoa.MaLoaiHangHoa
                   where (hanghoa.MaLoaiHangHoa.Equals(maLoaiHangHoa) && hanghoa.TrangThai == true)
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiaKhuyenMai = hanghoa.GiamGia,
                       TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       XuatXu = hanghoa.XuatXu,
                   }).AsEnumerable().Select(x => new HangHoaViewModel()
                   {
                       maHangHoa = x.MaHangHoa,
                       tenHangHoa = x.TenHangHoa,
                       hinhAnh = x.HinhAnh,
                       giaBan = x.GiaBan,
                       giamGia = x.GiaKhuyenMai,
                       tenLoaiHangHoa = x.TenLoaiHangHoa,
                       xuatXu = x.XuatXu,
                   }).ToList();
            return all;
        }

        public object TongSanPhamKhuyenMai()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       where hanghoa.TrangThai.Equals(true) && hanghoa.GiamGia > 0
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Distinct().Count();
            return all;
        }

        public IEnumerable<HangHoa> SanPhamKhuyenMai()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoa> all = new List<HangHoa>();

            all = (from hanghoa in danhSachHangHoa
                   where (hanghoa.TrangThai == true && hanghoa.GiamGia > 0)
                   orderby hanghoa.MaHangHoa descending
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       XuatXu = hanghoa.XuatXu,
                   }).AsEnumerable().Select(x => new HangHoa()
                   {
                       MaHangHoa = x.MaHangHoa,
                       TenHangHoa = x.TenHangHoa,
                       HinhAnh = x.HinhAnh,
                       GiaBan = x.GiaBan,
                       GiamGia = x.GiamGia,
                       XuatXu = x.XuatXu,
                   }).ToList();
            return all;
        }

        public HangHoa ViewDetail(int id)
        {
            return _dbContext.HangHoas.Find(id);
        }

        public object SanPhamHetHang()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       where hanghoa.SoLuongTon.Equals(0) && hanghoa.TrangThai.Equals(true)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Count();
            return all;
        }

        public object SanPhamSapHetHang()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       where hanghoa.SoLuongTon < 5 && hanghoa.SoLuongTon > 0 && hanghoa.TrangThai.Equals(true)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Count();
            return all;
        }

        public object SanPhamDangKinhDoanh()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       where hanghoa.TrangThai.Equals(true)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Count();
            return all;
        }

        public object SanPhamNgungKinhDoanh()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            var all = (from hanghoa in danhSachHangHoa
                       where hanghoa.TrangThai.Equals(false)
                       select new
                       {
                           MaHangHoa = hanghoa.MaHangHoa,
                       }).AsEnumerable().Select(x => new HangHoa()
                       {
                           MaHangHoa = x.MaHangHoa,
                       }).Count();
            return all;
        }

        public object TongSanPham()
        {
            return _hangHoaRepo.GetAll().Count();
        }

        public IList<SanPhamBanChayTaiCuaHangViewModel> SanPhamBanChayNhatTaiCuaHang()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            IQueryable<ChiTietPhieuBanHang> danhSachchiTietPhieuBanHang = _chiTietPhieuBanHangRepo.GetAll();
            List<SanPhamBanChayTaiCuaHangViewModel> all = new List<SanPhamBanChayTaiCuaHangViewModel>();

            var phieubanhangs = (from phieubanhang in danhSachchiTietPhieuBanHang
                                 group phieubanhang by phieubanhang.MaHangHoa into phieubanhangGroup
                                 orderby phieubanhangGroup.Sum(i => i.SoLuong) descending
                                 select new
                                 {
                                     MaHangHoa = phieubanhangGroup.Key,
                                     TongSoLuongBan = phieubanhangGroup.Sum(i => i.SoLuong),
                                     TongTien = phieubanhangGroup.Sum(i => i.ThanhTien)
                                 }).Take(15).ToList();

            all = (from phieubanhang in phieubanhangs
                   join hanghoa in danhSachHangHoa
                  on phieubanhang.MaHangHoa equals hanghoa.MaHangHoa
                   select new SanPhamBanChayTaiCuaHangViewModel
                   {
                       maHangHoa = phieubanhang.MaHangHoa,
                       tongSoLuongBan = phieubanhang.TongSoLuongBan,
                       tongTienBan = phieubanhang.TongTien,
                       tenHangHoa = hanghoa.TenHangHoa,
                       hinhAnh = hanghoa.HinhAnh
                   }).ToList();
            return all;
        }

        public IList<SanPhamBanChayOnlineViewModel> DanhSachSanPhamBanChayNhatOnline()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            IQueryable<ChiTietPhieuDatHang> danhSachchiTietPhieuDatHang = _chiTietPhieuDatHangRepo.GetAll();
            List<SanPhamBanChayOnlineViewModel> all = new List<SanPhamBanChayOnlineViewModel>();

            var phieudathangs = (from phieudathang in danhSachchiTietPhieuDatHang
                                 group phieudathang by phieudathang.MaHangHoa into phieudathangGroup
                                 orderby phieudathangGroup.Sum(i => i.SoLuong) descending
                                 select new
                                 {
                                     MaHangHoa = phieudathangGroup.Key,
                                     TongSoLuongBan = phieudathangGroup.Sum(i => i.SoLuong),
                                     TongTien = phieudathangGroup.Sum(i => i.ThanhTien)
                                 }).Take(15).ToList();

            all = (from phieudathang in phieudathangs
                   join hanghoa in danhSachHangHoa
                  on phieudathang.MaHangHoa equals hanghoa.MaHangHoa
                   select new SanPhamBanChayOnlineViewModel
                   {
                       maHangHoa = phieudathang.MaHangHoa,
                       tongSoLuongBan = phieudathang.TongSoLuongBan,
                       tongTienBan = phieudathang.TongTien,
                       tenHangHoa = hanghoa.TenHangHoa,
                       hinhAnh = hanghoa.HinhAnh
                   }).ToList();
            return all;
        }

        public IEnumerable<HangHoa> DanhSachHangHoaBanChayNhat()
        {
            var orders = (from od in _chiTietPhieuBanHangRepo.GetAll().GroupBy(m => m.MaHangHoa)
                          join hanghoa in _hangHoaRepo.GetAll()
                          on od.Key equals hanghoa.MaHangHoa
                          where (hanghoa.TrangThai == true)
                          select new
                          {
                              MaHangHoa = od.Key,
                              SoLuong = od.Sum(m => m.SoLuong),
                              HinhAnh = hanghoa.HinhAnh,
                              TenHangHoa = hanghoa.TenHangHoa,
                              GiaBan = hanghoa.GiaBan,
                              GiamGia = hanghoa.GiamGia,
                              XuatXu = hanghoa.XuatXu,
                          }).AsEnumerable().Select(x => new HangHoa()
                          {
                              MaHangHoa = x.MaHangHoa,
                              SoLuongTon = x.SoLuong,
                              TenHangHoa = x.TenHangHoa,
                              HinhAnh = x.HinhAnh,
                              GiamGia = x.GiamGia,
                              GiaBan = x.GiaBan,
                              XuatXu = x.XuatXu,
                          }).Distinct().Take(8).ToList();
            return orders;
        }

        public IEnumerable<HangHoa> DanhSachHangHoaGiamGia()
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoa> all = new List<HangHoa>();

            all = (from hanghoa in danhSachHangHoa
                   where (hanghoa.TrangThai == true && hanghoa.GiamGia > 0)
                   orderby hanghoa.MaHangHoa descending
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       XuatXu = hanghoa.XuatXu,
                   }).AsEnumerable().Select(x => new HangHoa()
                   {
                       MaHangHoa = x.MaHangHoa,
                       TenHangHoa = x.TenHangHoa,
                       HinhAnh = x.HinhAnh,
                       GiaBan = x.GiaBan,
                       GiamGia = x.GiamGia,
                       XuatXu = x.XuatXu,
                   }).Take(8).ToList();
            return all;
        }

        public IList<HangHoa> TimKiemHangHoa(string key)
        {
            IQueryable<HangHoa> danhSachHangHoa = _hangHoaRepo.GetAll();
            List<HangHoa> all = new List<HangHoa>();

            all = (from hanghoa in danhSachHangHoa
                   where hanghoa.TenHangHoa.ToLower().Contains(key)
                   select new
                   {
                       MaHangHoa = hanghoa.MaHangHoa,
                       TenHangHoa = hanghoa.TenHangHoa,
                       HinhAnh = hanghoa.HinhAnh,
                       GiaBan = hanghoa.GiaBan,
                       GiamGia = hanghoa.GiamGia,
                       XuatXu = hanghoa.XuatXu,
                   }).AsEnumerable().Select(x => new HangHoa()
                   {
                       MaHangHoa = x.MaHangHoa,
                       TenHangHoa = x.TenHangHoa,
                       HinhAnh = x.HinhAnh,
                       GiaBan = x.GiaBan,
                       GiamGia = x.GiamGia,
                       XuatXu = x.XuatXu,
                   }).ToList();
            return all;
        }
    }
}
