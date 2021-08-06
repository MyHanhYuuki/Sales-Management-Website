using Business.Interfaces;
using Common.Model;
using Common.Functions;
using Common.ViewModels;
using Data.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace Business.Implements
{
    public class ChucVuBusiness : IChucVuBusiness
    {
        private QLWebDBEntities _dbContext;
        private readonly ChucVuReponsitory _chucVuRepo;
        private readonly NhanVienReponsitory _nhanVienRepo;
        private readonly NhanVien_QuyenReponsitory _nhanVienQuyenRepo;
        private readonly PhanQuyenReponsitory _phanQuyenRepo;

        public ChucVuBusiness()
        {
            _dbContext = new QLWebDBEntities();
            _chucVuRepo = new ChucVuReponsitory(_dbContext);
            _nhanVienQuyenRepo = new NhanVien_QuyenReponsitory(_dbContext);
            _phanQuyenRepo = new PhanQuyenReponsitory(_dbContext);
            _nhanVienRepo = new NhanVienReponsitory(_dbContext);
        }
        
        /// <summary>
        /// Get menu from positionID
        /// </summary>
        /// <param name="positionID">positionID</param>
        /// <returns>list menu</returns>
        public List<NhanVien_QuyenViewModel> GetMenu(int? maChucVu)
        {
            if (maChucVu != null)
            {
                var lstMenu = (from nhanvienquyen in _nhanVienQuyenRepo.SearchFor(i => i.MaChucVu == maChucVu)
                               join phanquyen in _phanQuyenRepo.GetAll()
                               on nhanvienquyen.MaQuyen equals phanquyen.MaQuyen
                               select new NhanVien_QuyenViewModel
                               {
                                   maChucVu = nhanvienquyen.MaChucVu,
                                   maQuyen = nhanvienquyen.MaQuyen,
                                   parent = nhanvienquyen.ChuThich,
                                   tenQuyen = phanquyen.TenQuyen,
                               }).ToList();
                foreach (var item in lstMenu)
                {
                    item.controller = FindController.Controller(item.tenQuyen);
                }
                return new List<NhanVien_QuyenViewModel>(lstMenu);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get parent of child menu
        /// </summary>
        /// <param name="positionID">positionID</param>
        /// <returns>List<String></returns>
        public List<String> GetListParent(int? maChucvu)
        {
            if (maChucvu != null)
            {
                return _nhanVienQuyenRepo.SearchFor(i => i.MaChucVu == maChucvu).Select(x => x.ChuThich).Distinct().ToList();
            }
            else
            {
                return null;
            }
        }

        public List<Object> LoadChucVu()
        {
            IQueryable<ChucVu> dsChucVu = _chucVuRepo.GetAll();
            int[] IDs = {3, 4, 5, 6, 7 };
            
            var list = (from chucvu in dsChucVu
                        where IDs.Contains(chucvu.MaChucVu)
                        select new SelectListItem
                        {
                            Text = chucvu.TenChucVu,
                            Value = chucvu.MaChucVu.ToString(),
                        });
           
            return new List<Object>(list);
        }

        public List<Object> LoadChucVuTheoMaNhanVien(string maNhanVien)
        {
            IQueryable<ChucVu> dsChucVu = _chucVuRepo.GetAll();

            var list = (from chucvu in dsChucVu
                        join nhanvien in _nhanVienRepo.GetAll()
                        on chucvu.MaChucVu equals nhanvien.MaChucVu
                        where (nhanvien.MaNhanVien.Equals(maNhanVien))
                        select new SelectListItem
                        {
                            Text = chucvu.TenChucVu,
                            Value = chucvu.MaChucVu.ToString(),
                        });
            return new List<Object>(list);
        }

    }
}
