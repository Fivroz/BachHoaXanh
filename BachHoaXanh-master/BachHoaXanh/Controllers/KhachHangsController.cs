using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BachHoaXanh.Models;

namespace BachHoaXanh.Controllers
{
    public class KhachHangsController : Controller
    {
        private BACHHOAXANHEntities databases = new BACHHOAXANHEntities();

        public ActionResult Details(int id)
        {
            var ctKH = databases.KhachHangs.FirstOrDefault(s => s.MaKhachHang == id);
            return View(ctKH);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = databases.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.HangKhachHang = new SelectList(databases.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang", khachHang.HangKhachHang);
            return View(khachHang);
        }

        // POST: KDKhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKhachHang,HoTen,GioiTinh,SDT,DiaChi,MatKhau,TrangThai,HangKhachHang,DiemTichLuy")] KhachHang khachHang)
        {

            if (string.IsNullOrEmpty(khachHang.HoTen))
                ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
            if (string.IsNullOrEmpty(khachHang.SDT))
                ModelState.AddModelError(string.Empty, "SĐT không được để trống");
            if (string.IsNullOrEmpty(khachHang.DiaChi ))
                ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");

            if (!Regex.IsMatch(khachHang.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
                ModelState.AddModelError("HoTen", "Họ tên không được chứa ký tự đặc biệt và số.");
            
            if (!Regex.IsMatch(khachHang.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được chứa ký tự đặc biệt và số.");
    
            if (khachHang.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(khachHang.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDTNhanHang", "Số điện thoại không đúng định dạng.");

            var dm = databases.KhachHangs.FirstOrDefault(d => d.SDT == khachHang.SDT);
            if (dm != null && dm.MaKhachHang != khachHang.MaKhachHang)
                ModelState.AddModelError(string.Empty, "Đã tồn tại số điện thoại này này!");

            bool isNumeric = int.TryParse(khachHang.HoTen, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên khách hàng không được toàn số!");        
            isNumeric = int.TryParse(khachHang.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Địa chỉ không được toàn số!");
            var existingKhachHang = databases.KhachHangs.Find(khachHang.MaKhachHang);
            if (ModelState.IsValid)
            {
                if (existingKhachHang != null)
                {
                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.SDT = khachHang.SDT;
                    existingKhachHang.DiaChi = khachHang.DiaChi;
                    existingKhachHang.GioiTinh = khachHang.GioiTinh;
                }
                databases.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa thông tin thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            ViewBag.HangKhachHang = new SelectList(databases.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang", khachHang.HangKhachHang);
            return View(khachHang);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            if (string.IsNullOrEmpty(kh.HoTen))
                ModelState.AddModelError(string.Empty, "Họ tên không được để trống");
            if (string.IsNullOrEmpty(kh.SDT))
                ModelState.AddModelError(string.Empty, "SĐT không được để trống");
            if (string.IsNullOrEmpty(kh.DiaChi))
                ModelState.AddModelError(string.Empty, "Địa chỉ không được để trống");

            if (!Regex.IsMatch(kh.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
                ModelState.AddModelError("HoTen", "Họ tên không được chứa ký tự đặc biệt và số.");
            if (!Regex.IsMatch(kh.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không đúng định dạng");
            if (kh.HoTen.Length <= 8)
                ModelState.AddModelError("HoTen", "Họ tên phải có độ dài lớn hơn 8 kí tự.");

            if (kh.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(kh.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDTNhanHang", "Số điện thoại không đúng định dạng.");

            if (string.IsNullOrEmpty(kh.HoTen))
                ModelState.AddModelError("HoTen", "Họ tên không được để trống");
            if (string.IsNullOrEmpty(kh.SDT))
                ModelState.AddModelError("SDT", "Số điện thoại không được để trống");
            if (string.IsNullOrEmpty(kh.MatKhau))
                ModelState.AddModelError("MatKhau", "Mật khẩu không được để trống");
            if (string.IsNullOrEmpty(kh.DiaChi))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống");
            bool isNumeric = int.TryParse(kh.HoTen, out _);
            if (isNumeric)
                ModelState.AddModelError("HoTen", "Tên khách hàng không được toàn số!");
            isNumeric = int.TryParse(kh.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError("DiaChi", "Địa chỉ không được toàn số!");
            var khachhang = databases.KhachHangs.FirstOrDefault(k => k.SDT == kh.SDT);
            if (khachhang != null)
                ModelState.AddModelError("SDT", "Đã có người đăng kí số điện thoại này");
            if (ModelState.IsValid)
            {
                kh.TrangThai = false;
                kh.HangKhachHang = "TV";
                kh.DiemTichLuy = 0;
                databases.KhachHangs.Add(kh);
                databases.SaveChanges();
                Session["DANGKY"] = "Đăng ký thành công!";
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View(model);
        }

        // Action xử lý khi người dùng nhấn nút đổi mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if(string.IsNullOrEmpty(model.RetypedPassword))
                ModelState.AddModelError("RetypedPassword", "Mật khẩu hiện tại không được trống.");
            if (string.IsNullOrEmpty(model.NewPassword))
                ModelState.AddModelError("NewPassword", "Mật khẩu mới không được trống.");
            if (string.IsNullOrEmpty(model.ConfirmPassword))
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu nhập lại không được trống.");
            if (ModelState.IsValid)
            {
                KhachHang data = Session["User"] as KhachHang;
                // Thực hiện xác thực và cập nhật mật khẩu
                var kh = databases.KhachHangs.FirstOrDefault(s => s.MaKhachHang == data.MaKhachHang);

                if (kh != null)
                {
                    // Kiểm tra mật khẩu hiện tại của khách hàng
                    if (model.RetypedPassword != kh.MatKhau)
                    {
                        ModelState.AddModelError("", "Mật khẩu hiện tại không chính xác.");
                        return View(model);
                    }

                    // Kiểm tra mật khẩu mới và xác nhận mật khẩu
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Xác nhận mật khẩu không trùng khớp.");
                        return View(model);
                    }

                    // Cập nhật mật khẩu mới của khách hàng
                    kh.MatKhau = model.NewPassword;
                    databases.SaveChanges();

                    // Chuyển hướng đến trang xác nhận đổi mật khẩu
                    Session["CHANGEPWD"] = true;
                    string htmlContent = "<html><body><h3 class=\"text-center\">Đổi mật khẩu thành công^^</h3></body></html>";
                    return Content(htmlContent, "text/html");
                }
                else
                {
                    return HttpNotFound();
                }
            }

            return View(model);
        }


    }
}
