using Business.Implements;
using Common.Model;
using Common.Functions;
using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QLWeb.Controllers
{
    public class PhieuDatHangController : BaseController
    {
        readonly HangHoaBusiness _hangHoaBus = new HangHoaBusiness();
        readonly PhieuDatHangBusiness _phieuDatHangBus = new PhieuDatHangBusiness();
        readonly ChiTietPhieuDatHangBusiness _chiTietPhieuDatHangBus = new ChiTietPhieuDatHangBusiness();
        private const string CartSession = "CartSession";
        // GET: PhieuDatHang
        public ActionResult Index()
        {
            //Lấy Session cart ra để hiển thị sản phẩm trong giỏ hàng
            var cart = Session[CartSession];
            var list = new List<PhieuDatHangViewModel>();
            if (cart != null)
            {
                list = (List<PhieuDatHangViewModel>)cart;
            }
            return View(list);
        }

        public ActionResult DatHang(int maHangHoa, int soLuong)
        {
            // Kiểm tra Session cart xem có sản phẩm không
            // Nếu có thì chỉ tăng số lượng
            var product = _hangHoaBus.ViewDetail(maHangHoa);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<PhieuDatHangViewModel>)cart;
                if (list.Exists(x => x.maHangHoa == maHangHoa))
                {

                    foreach (var item in list)
                    {
                        if (item.maHangHoa == maHangHoa)
                        {
                            item.soLuong += soLuong;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new PhieuDatHangViewModel();
                    item.maHangHoa = product.MaHangHoa;
                    item.soLuong = soLuong;
                    item.tenHangHoa = product.TenHangHoa;
                    item.hinhAnh = product.HinhAnh;
                    item.giaBan = product.GiaBan;
                    item.giamGia = product.GiamGia;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new PhieuDatHangViewModel();
                item.maHangHoa = product.MaHangHoa;
                item.soLuong = soLuong;
                item.tenHangHoa = product.TenHangHoa;
                item.hinhAnh = product.HinhAnh;
                item.giaBan = product.GiaBan;
                item.giamGia = product.GiamGia;
                var list = new List<PhieuDatHangViewModel>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            Session[CartSession] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var sessionCart = (List<PhieuDatHangViewModel>)Session[CartSession];
            sessionCart.RemoveAll(x => x.maHangHoa == id);
            Session[CartSession] = sessionCart;
            return RedirectToAction("Index");
        }

        public ActionResult Update(FormCollection fc)
        {
            string[] quatities = fc.GetValues("soLuong");
            List<PhieuDatHangViewModel> cart = (List<PhieuDatHangViewModel>)Session[CartSession];
            for (int i = 0; i < cart.Count; i++)
                cart[i].soLuong = Convert.ToInt32(quatities[i]);
            Session[CartSession] = cart;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ThanhToan()
        {
            List<SelectListItem> hinhThucThanhToan = new List<SelectListItem>();
            hinhThucThanhToan.Add(new SelectListItem { Text = "Thanh toán tại cửa hàng" });
            hinhThucThanhToan.Add(new SelectListItem { Text = "Thanh toán tại nhà" });
            ViewBag.hinhThucThanhToan = hinhThucThanhToan;
            var cart = Session[CartSession];
            var list = new List<PhieuDatHangViewModel>();
            if (cart != null)
            {
                list = (List<PhieuDatHangViewModel>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public async Task<ActionResult> ThanhToan(PhieuDatHangViewModel datHang)
        {
            var phieuDatHang = new PhieuDatHang();
            phieuDatHang.NgayDat = DateTime.Now;
            phieuDatHang.TenKhachHang = datHang.tenKhachHang;
            phieuDatHang.SoDienThoai = datHang.soDienThoai;
            phieuDatHang.Diachi = datHang.diaChi;
            phieuDatHang.Email = datHang.email;
            phieuDatHang.HinhThucThanhToan = datHang.hinhThucThanhToan;
            phieuDatHang.Ghichu = datHang.ghiChu;
            phieuDatHang.NgayChinhSua = DateTime.Now;
            phieuDatHang.DaThanhToan = false;
            phieuDatHang.DaXacNhan = false;
            phieuDatHang.TrangThai = true;

            try
            {
                var soPhieuBanHang = _phieuDatHangBus.Insert(phieuDatHang);
                var cart = (List<PhieuDatHangViewModel>)Session[CartSession];
                decimal total = 0;
                foreach (var item in cart)
                {
                    var chiTietPhieuDatHang = new ChiTietPhieuDatHang();
                    chiTietPhieuDatHang.MaHangHoa = item.maHangHoa;
                    chiTietPhieuDatHang.SoPhieuDatHang = soPhieuBanHang;
                    chiTietPhieuDatHang.SoLuong = item.soLuong;
                    if (item.giamGia <= 0)
                    {
                        chiTietPhieuDatHang.Gia = item.giaBan;
                        chiTietPhieuDatHang.ThanhTien = item.giaBan * item.soLuong;
                        total += (item.giaBan * item.soLuong);
                    }
                    else
                    {
                        chiTietPhieuDatHang.Gia = item.giamGia;
                        chiTietPhieuDatHang.ThanhTien = item.giamGia * item.soLuong;
                        total += (item.giamGia * item.soLuong);
                    }

                    _chiTietPhieuDatHangBus.Insert(chiTietPhieuDatHang);
                }
                phieuDatHang.TongTien = total;
                _phieuDatHangBus.UpdateTongTien(phieuDatHang);
            }
            catch (Exception)
            {
                SetAlert("Đã xảy ra lỗi trong quá trình đặt hàng! Vui lòng thực hiện lại!!!", "error");
                return RedirectToAction("ThanhToan");
            }
            Session[CartSession] = null;
            return RedirectToAction("ThongBaoThanhCong");
        }

        public ActionResult ThongBaoThanhCong()
        {
            return View();
        }
    }
}