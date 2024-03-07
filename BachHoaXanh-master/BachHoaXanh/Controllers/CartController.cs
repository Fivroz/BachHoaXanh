using BachHoaXanh.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;

namespace BachHoaXanh.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        BACHHOAXANHEntities databases = new BACHHOAXANHEntities();

        public List<Cart> GetCart()
        { 
            List<Cart> cart = Session["Cart"] as List<Cart>;
            if (cart == null) {
                cart = new List<Cart>();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddProToCart(int MaSP)
        {
            //Lấy giỏ hàng hiện tại
            List<Cart> gioHang = GetCart();
            //Kiểm tra xem có tồn tại mặt hàng trong giỏ hay chưa
            //Nếu có thì tăng số lượng lên 1, ngược lại thêm vào giỏ
            Cart sanPham = gioHang.FirstOrDefault(s => s.MaSanPham == MaSP);
            if (sanPham == null) //Sản phẩm chưa có trong giỏ
            {
                sanPham = new Cart(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++; //Sản phẩm đã có trong giỏ thì tăng số lượng lên 1
                var checkSL = databases.SanPhams.FirstOrDefault(s => s.MaSanPham == MaSP);
                if (checkSL.SoLuongTon < sanPham.SoLuong) {
                    sanPham.SoLuong--;
                }
                if (sanPham.SoLuong > 50)
                {
                    sanPham.SoLuong = 50;
                    return RedirectToAction("TrangChu", "Home");
                }
            }
            return RedirectToAction("TrangChu", "Home");
        }
        private int Quantity()
        {
            int tongSL = 0;
            List<Cart> gioHang = GetCart();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }
        private double Price()
        {
            double TongTien = 0;
            List<Cart> gioHang = GetCart();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            return TongTien;
        }
        private int TinhTongKL()
        {
            int TongKL = 0;
            List<Cart> gioHang = GetCart();
            if (gioHang != null)
                TongKL = gioHang.Sum(sp => sp.TKhoiLuong());
            return TongKL;
        }
        public ActionResult ViewCart()
        {
            List<Cart> gioHang = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (gioHang == null || gioHang.Count == 0)
            {
                Session["Cart"] = null;
                return View();
            }
            else
            {
                ViewBag.TongSL = Quantity();
                ViewBag.TongTien = Price();
                double Phi = 0;
                decimal maxCanNang = decimal.MaxValue;
                decimal maxGiaPhi = 0;

                foreach (var item in databases.PhiGiaoHangs)
                {
                    if (item.CanNang >= TinhTongKL() && item.CanNang < maxCanNang)
                    {
                        maxCanNang = item.CanNang;
                        maxGiaPhi = item.GiaPhi;
                        Session["MaPhi"] = item.MaPhi;
                    }
                }

                if (maxGiaPhi > 0)
                    Phi = (int)maxGiaPhi;

                if (maxCanNang > 40000)
                {
                    double temp = Price();
                    Phi = temp * 0.03;
                }
                ViewBag.PhiGiaoHang = Phi;
                Session["PhiGiaoHang"] = Phi;
            }
            return View(gioHang); //Trả về View hiển thị thông tin giỏ hàng
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            List<Cart> gioHang = GetCart();
            var sanpham = gioHang.FirstOrDefault(s => s.MaSanPham == productId);
            if (sanpham != null)
            {
                var product = databases.SanPhams.FirstOrDefault(p => p.MaSanPham == productId);
                if (product != null)
                {
                    if (product.SoLuongTon >= quantity)
                    {
                        sanpham.SoLuong = quantity;
                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false });
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = Quantity();
            ViewBag.TongTien = Price();
            return PartialView();
        }
        public ActionResult Index()
        {
            return View();
        }      
        public ActionResult Create()
        {           
            KhachHang khach = Session["User"] as KhachHang; //Khách
            List<Cart> gioHang = GetCart();
            if (gioHang == null || gioHang.Count == 0) //Chưa có giỏ hàng hoặc chưa có sp
                return RedirectToAction("TrangChu", "Home");
            ViewBag.PhuongThucThanhToan = new SelectList(databases.PhuongThucThanhToans, "MaPhuongThuc", "TenPhuongThuc");
            DonDatHang DonHang = new DonDatHang();
            if (khach != null)
            {

                DonHang.DiaChiNhanHang = khach.DiaChi;
                DonHang.SDTNhanHang = khach.SDT;
                return View(DonHang);
            }
            return View();
        }

        // POST: DonDatHangs1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonDatHang DonHang)
        {
            KhachHang khach = Session["User"] as KhachHang; //Khách
            List<Cart> gioHang = GetCart();
            bool isNumeric = int.TryParse(DonHang.DiaChiNhanHang, out _);
            if (isNumeric)
                ModelState.AddModelError("DiaChiNhanHang", "Địa chỉ không được toàn số!");
            if (DonHang.SDTNhanHang.Length != 10)
                ModelState.AddModelError("SDTNhanHang", "Số điện thoại không đúng ký tự.");
            if (ModelState.IsValid)
            {
                ViewBag.TongSL = Quantity();
                ViewBag.TongTien = Price();
                DonHang.MaKhachHang = khach.MaKhachHang;
                DonHang.NgayDat = DateTime.Now;
                DonHang.TongTien = Convert.ToDecimal(Price()) + Convert.ToDecimal(Session["PhiGiaoHang"]);
                DonHang.TrangThaiDonHang = "Đã xác nhận";
                DonHang.PhiGiaoHang = Convert.ToInt16(Session["MaPhi"]);
                databases.DonDatHangs.Add(DonHang);
                databases.SaveChanges();
                foreach (var sanpham in gioHang)
                {
                    ChiTietDatHang chitiet = new ChiTietDatHang();
                    chitiet.MaDonHang = DonHang.MaDonHang;
                    chitiet.MaSanPham = sanpham.MaSanPham;
                    chitiet.SoLuong = sanpham.SoLuong;
                    chitiet.ThanhTien = (decimal)sanpham.DonGia * sanpham.SoLuong;
                    databases.ChiTietDatHangs.Add(chitiet);
                }
                databases.SaveChanges();
                Session["Cart"] = null;
                string htmlContent = "<html><body><h3>Quý khách đã đặt hàng thành công ^^ Cảm ơn Quý khách đã tin tưởng</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            ModelState.AddModelError("", "Đặt hàng thất bại!");
            ViewBag.PhuongThucThanhToan = new SelectList(databases.PhuongThucThanhToans, "MaPhuongThuc", "TenPhuongThuc", DonHang.PhuongThucThanhToan);
            return View(DonHang);
        }
        public ActionResult DeleteCart() 
        {
            List<Cart> gioHang = GetCart();
            gioHang.Clear();
            return RedirectToAction("TrangChu", "Home");
        }
        public ActionResult DeleteOne(int MaSP)
        {
            List<Cart> gioHang = GetCart();
            //Lấy sản phẩm trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(s => s.MaSanPham == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaSanPham == MaSP);
                return RedirectToAction("ViewCart"); //Quay về trang giỏ hàng
            }
            if (gioHang.Count == 0) //Quay về trang chủ nếu giỏ hàng không có gì
                return RedirectToAction("TrangChu", "Home");
            return RedirectToAction("ViewCart");
        }

    }
}