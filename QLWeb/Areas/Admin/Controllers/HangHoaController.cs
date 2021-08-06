using Business.Implements;
using Common.Model;
using Common.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLWeb.Areas.Admin.Controllers
{
    public class HangHoaController : BaseController
    {
        readonly HangHoaBusiness _hangHoaKhoBus = new HangHoaBusiness();
        readonly LoaiHangHoaBusiness _loaiHangHoaKhoBus = new LoaiHangHoaBusiness();

        // GET: Admin/LoaiHangHoa
        public ActionResult Index()
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Toàn bộ", Value = null });
            trangThai.Add(new SelectListItem { Text = "Đang Kinh Doanh", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Ngừng Kinh Doanh", Value = "false" });
            ViewBag.data = trangThai;

            return View();
        }

        public ActionResult DanhSachHangHoa(string searchString, string trangThai, int page = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return View(_hangHoaKhoBus.SearchDanhSachHangHoa(searchString, trangThai).ToPagedList(page, pageSize));
            }
            if (!string.IsNullOrEmpty(trangThai))
            {
                return View(_hangHoaKhoBus.SearchDanhSachHangHoa(searchString, trangThai).ToPagedList(page, pageSize));
            }

            return View(_hangHoaKhoBus.LoadDanhSachHangHoa().ToPagedList(page, pageSize));
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.loaihanghoa = _loaiHangHoaKhoBus.LoadTenLoaiHangHoa();
            return View();
        }

        [HttpGet]
        public JsonResult CheckHangHoa(string tenhanghoa, string modelname)
        {
            var isDuplicate = false;

            foreach (var user in _hangHoaKhoBus.GetAllHangHoa())
            {
                if (user.TenHangHoa == tenhanghoa)
                    isDuplicate = true;
                if (user.ModelName == modelname)
                    isDuplicate = true;
            }

            var jsonData = new { isDuplicate };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(HangHoaViewModel hangHoa, HttpPostedFileBase hinhAnh)
        {
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/client/product"),
                                               Path.GetFileName(hinhAnh.FileName));
                    hinhAnh.SaveAs(path);
                    hangHoa.hinhAnh = hinhAnh.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                hangHoa.hinhAnh = "default.png";
            }

            try
            {
                await _hangHoaKhoBus.Create(hangHoa);
                SetAlert("Đã thêm hàng hóa thành công!!!", "success");
            }
            catch
            {
                TempData["hangHoa"] = hangHoa;
                SetAlert("Đã xảy ra lỗi! Bạn hãy thêm lại", "error");
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Detail
        public ActionResult Detail(int id)
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Đang kinh doanh", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Ngừng kinh doanh", Value = "false" });
            ViewBag.data = trangThai;
            ViewBag.loaihanghoa = _loaiHangHoaKhoBus.LoadLoaiHangHoa();
            return View(_hangHoaKhoBus.LoadDanhSachHangHoaTheoMa(id).ToList());
        }

        public ActionResult ThongTinHangHoa(int id)
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Đang kinh doanh", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Ngừng kinh doanh", Value = "false" });
            ViewBag.data = trangThai;
            ViewBag.loaihanghoa = _loaiHangHoaKhoBus.LoadTenLoaiHangHoa();

            ViewBag.thongTinHangHoa = _hangHoaKhoBus.LoadDanhSachHangHoaTheoMa(id).ToList();

            return View(ViewBag.thongTinHangHoa);
        }

        // GET: Admin/Edit
        public ActionResult Edit(int id)
        {
            List<SelectListItem> trangThai = new List<SelectListItem>();
            trangThai.Add(new SelectListItem { Text = "Đang kinh doanh", Value = "true" });
            trangThai.Add(new SelectListItem { Text = "Ngừng kinh doanh", Value = "false" });
            ViewBag.data = trangThai;
            ViewBag.loaihanghoa = _loaiHangHoaKhoBus.LoadLoaiHangHoa();
            return View(_hangHoaKhoBus.LoadDanhSachHangHoaTheoMa(id).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, HangHoaViewModel hangHoa, HttpPostedFileBase hinhAnh)
        {
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Content/client/product"),
                                               Path.GetFileName(hinhAnh.FileName));
                    hinhAnh.SaveAs(path);
                    hangHoa.hinhAnh = hinhAnh.FileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                // hangHoa.hinhAnh = "default.png";
                hangHoa.hinhAnh = hangHoa.checkImage;
            }

            //Get hàng hóa muốn update (find by ID)
            HangHoa edit = (HangHoa)await _hangHoaKhoBus.Find(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Access Update from Business
                try
                {
                    await _hangHoaKhoBus.Update(hangHoa, edit);
                    SetAlert("Đã cập nhật hàng hóa thành công!!!", "success");

                }
                catch
                {
                    TempData["hangHoa"] = hangHoa;
                    SetAlert("Đã xảy ra lỗi! Bạn hãy cập nhật lại", "error");
                }
            }
            return RedirectToAction("Index");
        }
    }
}