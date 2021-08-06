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
    public class LoaiHangHoaController : BaseController
    {
        readonly LoaiHangHoaBusiness _loaiHangHoaKhoBus = new LoaiHangHoaBusiness();

        // GET: Admin/LoaiHangHoa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DanhSachLoaiHangHoa(string searchString, int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(_loaiHangHoaKhoBus.SearchDanhSachLoaiHangHoa(searchString).ToPagedList(page, pageSize));
            }

            return View(_loaiHangHoaKhoBus.LoadDanhSachLoaiHangHoa().ToPagedList(page, pageSize));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CheckTenLoaiHangHoa(string tenloaihanghoa)
        {
            var isDuplicate = false;

            foreach (var user in _loaiHangHoaKhoBus.GetAllTenLoaiHangHoa())
            {
                if (user.TenLoaiHangHoa == tenloaihanghoa)
                    isDuplicate = true;
            }

            var jsonData = new { isDuplicate };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(LoaiHangHoaViewModel loaiHangHoa)
        {
            try
            {
                await _loaiHangHoaKhoBus.Create(loaiHangHoa);
                SetAlert("Đã thêm loại hàng hóa thành công!!!", "success");
            }
            catch
            {
                TempData["loaiHangHoa"] = loaiHangHoa;
                SetAlert("Đã xảy ra lỗi! Bạn hãy thêm lại", "error");
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Detail
        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult ThongTinLoaiHangHoa(int id)
        {
            ViewBag.thongTinLoaiHangHoa = _loaiHangHoaKhoBus.LoadDanhSachLoaiHangHoaTheoMa(id).ToList();
            return View(ViewBag.thongTinLoaiHangHoa);
        }

        // GET: Admin/Edit
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, LoaiHangHoaViewModel loaiHangHoa)
        {
            //Get loại hàng hóa muốn update (find by ID)
            LoaiHangHoa edit = (LoaiHangHoa)await _loaiHangHoaKhoBus.Find(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Access Update from Business
                try
                {
                    await _loaiHangHoaKhoBus.Update(loaiHangHoa, edit);
                    SetAlert("Đã cập nhật loại hàng hóa thành công!!!", "success");

                }
                catch
                {
                    TempData["loaiHangHoa"] = loaiHangHoa;
                    SetAlert("Đã xảy ra lỗi! Bạn hãy cập nhật lại", "error");
                }
            }
            return RedirectToAction("Index");
        }
    }
}