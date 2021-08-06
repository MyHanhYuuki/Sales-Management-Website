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
    public class XuatKhoController : BaseController
    {
        readonly PhieuXuatKhoBusiness _phieuXuatKhoBus = new PhieuXuatKhoBusiness();
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();
        readonly NhanVienBusiness _nhanVienBus = new NhanVienBusiness();

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

        public ActionResult DanhSachPhieuXuatKho(string searchString, string trangThai, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            return View(_phieuXuatKhoBus.SearchDanhSachPhieuXuatKho(searchString, trangThai, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), HomeController.userName).ToPagedList(page, pageSize));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.maNhanVien = _nhanVienBus.LoadMaNhanVien(HomeController.userName);
            ViewBag.tenNhanVien = _nhanVienBus.LoadTenNhanVien(HomeController.userName);
            ViewBag.soPhieuXuatKho = _phieuXuatKhoBus.LoadSoPhieuXuatKho();
            ViewBag.danhSachHangHoa = new SelectList(_hangHoaBus.LoadSanhSachHangHoaKho(), "Value", "Text");
            return View();
        }

        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = _hangHoaBus.LayThongTinHangHoa(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> LuuPhieuXuatKho(PhieuXuatKhoViewModel phieuXuatKho)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                await _phieuXuatKhoBus.Create(phieuXuatKho);
                status = true;
                SetAlert("Đã Lưu Phiếu Xuất Kho Thành Công!!!", "success");
            }
            else
            {
                status = false;
                SetAlert("Đã Xảy Ra Lỗi! Bạn Hãy Tạo Lại Phiếu Xuất Kho", "error");
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
            PhieuXuatKho huyPhieuXuatKho = (PhieuXuatKho)await _phieuXuatKhoBus.Find(id);

            if (huyPhieuXuatKho == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuXuatKhoBus.HuyPhieuXuatKho(huyPhieuXuatKho);
                    SetAlert("Đã hủy phiếu xuất kho thành công!!!", "success");
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

        public ActionResult ThongTinPhieuXuatKho(int id)
        {
            ViewBag.chiTietPhieuXuatKho = _phieuXuatKhoBus.thongTinChiTietPhieuXuatKhoTheoMa(id).ToList();
            ViewBag.phieuXuatKho = _phieuXuatKhoBus.thongTinPhieuXuatKhoTheoMa(id).ToList();
            return View();
        }
    }
}