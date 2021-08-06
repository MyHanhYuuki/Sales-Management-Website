using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Model;
using System.Web.Security;
using System.Net;
using System.Timers;
using Business.Implements;
using Common.ViewModels;
using System.Threading.Tasks;
using Common;
using Common.Functions;
using System.IO;
using PagedList;

namespace QLWeb.Areas.Admin.Controllers
{
    public class NhapKhoController : BaseController
    {
        readonly PhieuNhapKhoBusiness _phieuNhapKhoBus = new PhieuNhapKhoBusiness();
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();
        readonly NhanVienBusiness _nhanVienBus = new NhanVienBusiness();
        readonly NhaCungCapBusiness _nhaCungCapBus = new NhaCungCapBusiness();

        // GET: Admin/NhapKho
        public ActionResult Index()
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Toàn bộ", Value = null });
            trangThai.Add(new SelectListItem { Text = "Hoàn thành", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Đã hủy", Value = "false" });
            ViewBag.trangthai = trangThai;
            return View();
        }

        public ActionResult DanhSachPhieuNhap(string searchString, string trangThai, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            return View(_phieuNhapKhoBus.SearchDanhSachPhieuNhapKho(searchString, trangThai, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), HomeController.userName).ToPagedList(page, pageSize));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.maNhanVien = _nhanVienBus.LoadMaNhanVien(HomeController.userName);
            ViewBag.tenNhanVien = _nhanVienBus.LoadTenNhanVien(HomeController.userName);
            ViewBag.tenNhaCungCap = _nhaCungCapBus.LoadNhaCungCap();
            ViewBag.soPhieuNhap = _phieuNhapKhoBus.LoadSoPhieuNhap();
            ViewBag.danhSachHangHoa = new SelectList(_hangHoaBus.LoadSanhSachHangHoaKho(), "Value", "Text");
            return View();
        }

        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = _hangHoaBus.LayThongTinHangHoa(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> LuuPhieuNhap(PhieuNhapViewModel phieuNhap)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                await _phieuNhapKhoBus.Create(phieuNhap);
                status = true;
                SetAlert("Đã Lưu Phiếu Nhập Kho Thành Công!!!", "success");
            }
            else
            {
                status = false;
                SetAlert("Đã Xảy Ra Lỗi! Bạn Hãy Tạo Lại Phiếu Nhập Kho", "error");
            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: Admin/Delete
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Deletes(int id, string quaylai)
        {
            if (quaylai != null)
                return RedirectToAction("Index");
            PhieuNhap huyPhieuNhap = (PhieuNhap)await _phieuNhapKhoBus.Find(id);

            if (huyPhieuNhap == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuNhapKhoBus.HuyPhieuNhap(huyPhieuNhap);
                    SetAlert("Đã hủy phiếu nhập kho thành công!!!", "success");
                }
                catch
                {
                    SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                    return RedirectToAction("Edit");
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Detail
        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult ThongTinPhieuNhap(int id)
        {
            ViewBag.chiTietPhieuNhap = _phieuNhapKhoBus.thongTinChiTietPhieuNhapTheoMa(id).ToList();
            ViewBag.phieuNhap = _phieuNhapKhoBus.thongTinPhieuNhapTheoMa(id).ToList();
            return View();
        }
        public ActionResult LoadThongTinPhieuNhap(int id)
        {
            var result = _phieuNhapKhoBus.LayThongTinPhieuNhap(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}