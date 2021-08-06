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
    public class LoaiHangHoaBusiness : ILoaiHangHoaBusiness
    {
        private QLWebDBEntities _dbContext;
        private readonly LoaiHangHoaReponsitory _loaiHangHoaRepo;

        public LoaiHangHoaBusiness()
        {
            _dbContext = new QLWebDBEntities();
            _loaiHangHoaRepo = new LoaiHangHoaReponsitory(_dbContext);
        }

        public IList<LoaiHangHoa> LoadDSLoaiHangHoa()
        {
            IQueryable<LoaiHangHoa> loaiHangHoa = _loaiHangHoaRepo.GetAll();
            return loaiHangHoa.ToList();
        }

        public IList<LoaiHangHoaViewModel> SearchDanhSachLoaiHangHoa(String key)
        {
            IQueryable<LoaiHangHoa> danhSachLoaiHangHoa = _loaiHangHoaRepo.GetAll();
            List<LoaiHangHoaViewModel> all = new List<LoaiHangHoaViewModel>();

            all = (from loaihanghoa in danhSachLoaiHangHoa
                   where loaihanghoa.TenLoaiHangHoa.ToString().Contains(key)
                   select new
                   {
                       MaLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                       TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       PhanTramLoiNhuan = loaihanghoa.PhanTramLoiNhuan,
                   }).AsEnumerable().Select(x => new LoaiHangHoaViewModel()
                   {
                       maLoaiHangHoa = x.MaLoaiHangHoa,
                       tenLoaiHangHoa = x.TenLoaiHangHoa,
                       phanTramLoiNhuan = x.PhanTramLoiNhuan,
                   }).ToList();

            return all;

        }

        public object TongLoaiSanPham()
        {
            return _loaiHangHoaRepo.GetAll().Count();
        }

        public List<Object> LoadLoaiHangHoa()
        {
            IQueryable<LoaiHangHoa> dsLoaiHangHoa = _loaiHangHoaRepo.GetAll();
            var list = (from loaihanghoa in dsLoaiHangHoa
                        select new SelectListItem
                        {
                            Text = loaihanghoa.TenLoaiHangHoa,
                            Value = loaihanghoa.MaLoaiHangHoa.ToString(),
                        });
            return new List<Object>(list);
        }

        public IEnumerable<LoaiHangHoaViewModel> LoadDanhSachLoaiHangHoa()
        {
            IQueryable<LoaiHangHoa> danhSachLoaiHangHoa = _loaiHangHoaRepo.GetAll();
            List<LoaiHangHoaViewModel> all = new List<LoaiHangHoaViewModel>();

            all = (from loaihanghoa in danhSachLoaiHangHoa
                   select new
                   {
                       MaLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                       TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       PhanTramLoiNhuan = loaihanghoa.PhanTramLoiNhuan,
                   }).AsEnumerable().Select(x => new LoaiHangHoaViewModel()
                   {
                       maLoaiHangHoa = x.MaLoaiHangHoa,
                       tenLoaiHangHoa = x.TenLoaiHangHoa,
                       phanTramLoiNhuan = x.PhanTramLoiNhuan,
                   }).ToList();

            return all;

        }

        public IEnumerable<LoaiHangHoa> GetAllTenLoaiHangHoa()
        {
            IQueryable<LoaiHangHoa> danhSachLoaiHangHoa = _loaiHangHoaRepo.GetAll();

            var all = (from loaihanghoa in danhSachLoaiHangHoa
                       select new
                       {
                           TenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                       }).AsEnumerable().Select(x => new LoaiHangHoa()
                       {
                           TenLoaiHangHoa = x.TenLoaiHangHoa,
                       }).ToList();
            return all;
        }

        public async Task Create(object model)
        {
            var loaiHangHoa = new LoaiHangHoa();
            LoaiHangHoaViewModel input = (LoaiHangHoaViewModel)model;

            loaiHangHoa.TenLoaiHangHoa = input.tenLoaiHangHoa;
            loaiHangHoa.PhanTramLoiNhuan = input.phanTramLoiNhuan;

            await _loaiHangHoaRepo.InsertAsync(loaiHangHoa);
        }

        public IEnumerable<LoaiHangHoaViewModel> LoadDanhSachLoaiHangHoaTheoMa(int maloaihanghoa)
        {
            IQueryable<LoaiHangHoa> danhSachLoaiHangHoa = _loaiHangHoaRepo.GetAll();

            var all = (from loaihanghoa in danhSachLoaiHangHoa
                       where (loaihanghoa.MaLoaiHangHoa.Equals(maloaihanghoa))
                       select new LoaiHangHoaViewModel
                       {
                           maLoaiHangHoa = loaihanghoa.MaLoaiHangHoa,
                           tenLoaiHangHoa = loaihanghoa.TenLoaiHangHoa,
                           phanTramLoiNhuan = loaihanghoa.PhanTramLoiNhuan,
                       }).ToList();

            return all;
        }

        public async Task<object> Find(int ID)
        {
            return await _loaiHangHoaRepo.GetByIdAsync(ID);
        }

        public async Task Update(object inputModel, object editModel)
        {
            LoaiHangHoaViewModel input = (LoaiHangHoaViewModel)inputModel;
            LoaiHangHoa editLoaiHangHoa = (LoaiHangHoa)editModel;

            editLoaiHangHoa.TenLoaiHangHoa = input.tenLoaiHangHoa;
            editLoaiHangHoa.PhanTramLoiNhuan = input.phanTramLoiNhuan;

            await _loaiHangHoaRepo.EditAsync(editLoaiHangHoa);
        }

        public List<Object> LoadTenLoaiHangHoa()
        {
            IQueryable<LoaiHangHoa> dsLoaiHangHoa = _loaiHangHoaRepo.GetAll();

            var list = (from loaihanghoa in dsLoaiHangHoa
                        select new SelectListItem
                        {
                            Text = loaihanghoa.TenLoaiHangHoa,
                            Value = loaihanghoa.MaLoaiHangHoa.ToString(),
                        });
            return new List<Object>(list);
        }        

    }
}
