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
    public class BaoHanhController : BaseController
    {
        readonly PhieuBaoHanhBusiness _phieuBaoHanhBus = new PhieuBaoHanhBusiness();
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();
        readonly NhanVienBusiness _nhanVienBus = new NhanVienBusiness();

        // GET: Admin/PhieuBaoHanh
        public ActionResult Index()
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Toàn bộ", Value = null });
            trangThai.Add(new SelectListItem { Text = "Đã tạo", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Đã hủy", Value = "false" });
            ViewBag.trangthai = trangThai;
            return View();
        }

        public ActionResult DanhSachPhieuBaoHanh(string searchString, string trangThai, int page = 1, int pageSize = 10)
        {
            return View(_phieuBaoHanhBus.SearchDanhSachPhieuBaoHanh(searchString, trangThai, HomeController.userName).ToPagedList(page, pageSize));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.maNhanVien = _nhanVienBus.LoadMaNhanVien(HomeController.userName);
            ViewBag.tenNhanVien = _nhanVienBus.LoadTenNhanVien(HomeController.userName);
            ViewBag.soPhieuBaoHanh = _phieuBaoHanhBus.LoadSoPhieuBaoHanh();
            ViewBag.danhSachHangHoa = new SelectList(_hangHoaBus.LoadSanhSachHangHoaKho(), "Value", "Text");
            return View();
        }

        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = _hangHoaBus.LayThongTinHangHoa(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> LuuPhieuBaoHanh(PhieuBaoHanhViewModel phieuBaoHanh)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                await _phieuBaoHanhBus.Create(phieuBaoHanh);
                status = true;
                SetAlert("Đã Lưu Phiếu Bảo Hành Thành Công!!!", "success");
            }
            else
            {
                status = false;
                SetAlert("Đã Xảy Ra Lỗi! Bạn Hãy Tạo Lại Phiếu Bảo Hành", "error");
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
            PhieuBaoHanh huyPhieuBaoHanh = (PhieuBaoHanh)await _phieuBaoHanhBus.Find(id);

            if (huyPhieuBaoHanh == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuBaoHanhBus.HuyPhieuBaoHanh(huyPhieuBaoHanh);
                    SetAlert("Đã hủy phiếu bảo hành thành công!!!", "success");
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

        // GET: Admin/Confirm
        public ActionResult Confirm()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Confirms(int id, string quaylai)
        {
            if (quaylai != null)
                return RedirectToAction("Index");
            PhieuBaoHanh xacNhanPhieuBaoHanh = (PhieuBaoHanh)await _phieuBaoHanhBus.Find(id);

            if (xacNhanPhieuBaoHanh == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuBaoHanhBus.XacNhanPhieuBaoHanh(xacNhanPhieuBaoHanh);
                    SetAlert("Đã xác nhận phiếu bảo hành thành công!!!", "success");
                }
                catch
                {
                    SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                    return RedirectToAction("Edit");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongTinPhieuBaoHanh(int id)
        {
            ViewBag.chiTietPhieuBaoHanh = _phieuBaoHanhBus.thongTinChiTietPhieuBaoHanhTheoMa(id).ToList();
            ViewBag.phieuBaoHanh = _phieuBaoHanhBus.thongTinPhieuBaoHanhTheoMa(id).ToList();
            return View();
        }

    }
}