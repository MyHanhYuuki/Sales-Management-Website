using Business.Implements;
using Common.Model;
using Common.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLWeb.Areas.Admin.Controllers
{
    public class PhieuChiController : BaseController
    {
        readonly PhieuChiBusiness _phieuChiBus = new PhieuChiBusiness();
        readonly NhanVienBusiness _nhanVienBus = new NhanVienBusiness();
        readonly PhieuNhapKhoBusiness _phieuNhapBus = new PhieuNhapKhoBusiness();
        // GET: Admin/PhieuChi
        public ActionResult Index()
        {
            ViewBag.trangthai = new SelectList(new[]{ new { Value = "true", Text = "Hoàn thành" },
                                                    new { Value = "false", Text = "Đã hủy" }},
                                              "Value", "Text");
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.maNhanVien = _nhanVienBus.LoadMaNhanVien(HomeController.userName);
            ViewBag.tenNhanVien = _nhanVienBus.LoadTenNhanVien(HomeController.userName);
            ViewBag.soPhieuChiTuTang = _phieuChiBus.LoadSoPhieuChi();
            
            ViewBag.danhSachPhieuNhap = new SelectList(_phieuChiBus.LoadDanhSachPhieuNhap(), "Value", "Text");

            return View();
        }

        public ActionResult LayTongTienPhieuNhap(int id)
        {
            var results = _phieuNhapBus.LayThongTinPhieuNhap(id);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> LuuPhieuChi(PhieuChiViewModel phieuChi)
        {
            if (ModelState.IsValid)
            {
                await _phieuChiBus.Create(phieuChi);
                SetAlert("Đã Lưu Phiếu Chi Thành Công!!!", "success");
            }
            else
            {
                SetAlert("Đã Xảy Ra Lỗi! Bạn Hãy Tạo Lại Phiếu Chi", "error");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
        public ActionResult Detail()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Deletes(int id)
        {
            PhieuChi huyPhieuChi = (PhieuChi)await _phieuChiBus.Find(id);

            if (huyPhieuChi == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuChiBus.HuyPhieuChi(huyPhieuChi);
                    SetAlert("Đã hủy phiếu chi thành công!!!", "success");
                }
                catch
                {
                    SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                    return RedirectToAction("Edit");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DanhSachPhieuChi(string searchString, string trangthai, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            return View(_phieuChiBus.SearchDanhSachPhieuChi(searchString, trangthai, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), HomeController.userName).ToPagedList(page, pageSize));
        }

        public ActionResult ThongTinPhieuChi(int id)
        {
            ViewBag.phieuChi = _phieuChiBus.thongTinPhieuChiTheoMa(id).ToList();
            return View();
        }
      

    }
}