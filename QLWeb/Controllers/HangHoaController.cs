using Business.Implements;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLWeb.Controllers
{
    public class HangHoaController : Controller
    {
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();

        // GET: HangHoa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var sanpham = _hangHoaBus.LoadHangHoaTheoMa(id);
            return View(sanpham);
        }

        public ActionResult DanhSachSanPham(int id, int page = 1, int pageSize = 8)
        {
            ViewBag.tenLoaiHangHoa = _hangHoaBus.TenLoaiHangHoaTheoMaLoaiHangHoa(id);
            ViewBag.tongSanPham = _hangHoaBus.TongSanPhamTheoLoaiHang(id);
            var danhsachsanpham = _hangHoaBus.DanhSachHangHoaTheoMaLoaiHangHoa(id).ToPagedList(page, pageSize);
            return View(danhsachsanpham);
        }

        public ActionResult SanPhamKhuyenMai(int page = 1, int pageSize = 8)
        {
            ViewBag.tongSanPham = _hangHoaBus.TongSanPhamKhuyenMai();
            var sanpham = _hangHoaBus.SanPhamKhuyenMai().ToPagedList(page, pageSize);
            return View(sanpham);
        }
    }
}