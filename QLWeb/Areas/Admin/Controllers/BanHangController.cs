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
    public class BanHangController : BaseController
    {
        readonly PhieuBanHangBusiness _phieuBanHangBus = new PhieuBanHangBusiness();
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();
        readonly NhanVienBusiness _nhanVienBus = new NhanVienBusiness();
        // GET: Admin/PhieuBanHang

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
            ViewBag.soPhieuBanHangTuTang = _phieuBanHangBus.LoadSoPhieuBanHang();
            ViewBag.danhSachHangHoa = new SelectList(_hangHoaBus.LoadSanhSachHangHoaKho(), "Value", "Text");
            return View();
        }
        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = _hangHoaBus.LayThongTinHangHoa(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ListName(string q)
        //{
        //    var data = _hangHoaBus.ListName(q);
        //    return Json(new
        //    {
        //        data = data,
        //        status = true
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<JsonResult> LuuPhieuBanHang(PhieuBanHangViewModel phieuBanHang)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                await _phieuBanHangBus.Create(phieuBanHang);
                status = true;
                SetAlert("Đã Lưu Phiếu Bán Hàng Thành Công!!!", "success");
                
            }
            else
                SetAlert("Đã Xảy Ra Lỗi! Bạn Hãy Tạo Lại Phiếu Bán Hàng", "error");
            return new JsonResult { Data = new { status = status } };
        }

       
        public ActionResult DanhSachPhieuBanHang(string searchString, string trangthai, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            var res = _phieuBanHangBus.SearchDanhSachPhieuBanHang(searchString, trangthai, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), HomeController.userName).ToPagedList(page, pageSize);
            return View(res);
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        //[HttpPost]
        public async Task<ActionResult> Deletes(int id)
        {
            PhieuBanHang huyPhieuBanHang = (PhieuBanHang)await _phieuBanHangBus.Find(id);

            if (huyPhieuBanHang == null)
            {
                SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                return RedirectToAction("Edit");
            }
            else
            {
                try
                {
                    await _phieuBanHangBus.HuyPhieuBanHang(huyPhieuBanHang);
                    SetAlert("Đã hủy phiếu kiểm kho thành công!!!", "success");
                }
                catch
                {
                    SetAlert("Đã xảy ra lỗi! Bạn hãy hủy lại", "error");
                    return RedirectToAction("Edit");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThongTinPhieuBanHang(int id)
        {
            ViewBag.chiTietPhieuBanHang = _phieuBanHangBus.danhSachPhieuBanHangTheoMa(id).ToList();
            ViewBag.phieuBanHang = _phieuBanHangBus.thongTinPhieuBanHangTheoMa(id).ToList();
            return View();
        }

    }
}