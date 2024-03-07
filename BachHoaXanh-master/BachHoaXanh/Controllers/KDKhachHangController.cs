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
    public class KDKhachHangController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: KDKhachHang
        public ActionResult Index()
        {
            var khachHangs = db.KhachHangs.Include(k => k.HangKhachHang1);
            return View(khachHangs.ToList());
        }

        // GET: KDKhachHang/Create
        public ActionResult Create()
        {
            ViewBag.HangKhachHang = new SelectList(db.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang");
            return View();
        }

        // POST: KDKhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( KhachHang khachHang)
        {
            if (string.IsNullOrEmpty(khachHang.HoTen))
                ModelState.AddModelError("HoTen", "Họ tên không được để trống");

            if (string.IsNullOrEmpty(khachHang.SDT))
                ModelState.AddModelError("SDT", "SĐT không được để trống");

            if (string.IsNullOrEmpty(khachHang.DiaChi))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống");

            if (!Regex.IsMatch(khachHang.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
                ModelState.AddModelError("HoTen", "Họ tên không được chứa ký tự đặc biệt và số.");

            if (khachHang.HoTen.Length <= 8)
                ModelState.AddModelError("HoTen", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(khachHang.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDTNhanHang", "Số điện thoại không đúng định dạng."); 
            if (!Regex.IsMatch(khachHang.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được chứa ký tự đặc biệt và số.");

            if (khachHang.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");
      
            var dm = db.KhachHangs.FirstOrDefault(d => d.SDT == khachHang.SDT);
            if (dm != null)
                ModelState.AddModelError("SDT", "Đã tồn tại số điện thoại này!");
            bool isNumeric = int.TryParse(khachHang.HoTen, out _);
            if (isNumeric)
                ModelState.AddModelError("HoTen", "Tên khách hàng không được toàn số!");
            isNumeric = int.TryParse(khachHang.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError("DiaChi", "Địa chỉ không được toàn số!");
            if (ModelState.IsValid)
            {
                khachHang.TrangThai = false;
                khachHang.HangKhachHang = "TV";
                khachHang.DiemTichLuy = 0;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm khách hàng thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(khachHang);
        }

        // GET: KDKhachHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.HangKhachHang = new SelectList(db.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang", khachHang.HangKhachHang);
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
                ModelState.AddModelError("HoTen", "Họ tên không được để trống");

            if (string.IsNullOrEmpty(khachHang.SDT))
                ModelState.AddModelError("SDT", "SĐT không được để trống");

            if (string.IsNullOrEmpty(khachHang.DiaChi))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống");

            if (!Regex.IsMatch(khachHang.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
                ModelState.AddModelError("HoTen", "Họ tên không được chứa ký tự đặc biệt và số.");

            if (khachHang.HoTen.Length <= 8)
                ModelState.AddModelError("HoTen", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(khachHang.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDTNhanHang", "Số điện thoại không đúng định dạng.");

            if (!Regex.IsMatch(khachHang.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được chứa ký tự đặc biệt và số.");

            if (khachHang.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");


            var dm = db.KhachHangs.FirstOrDefault(d => d.SDT == khachHang.SDT);
            if (dm != null && dm.MaKhachHang != khachHang.MaKhachHang)
                ModelState.AddModelError(string.Empty, "Đã tồn tại số điện thoại này này!");
            bool isNumeric = int.TryParse(khachHang.HoTen, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên khách hàng không được toàn số!");
            isNumeric = int.TryParse(khachHang.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Địa chỉ không được toàn số!");
            if (ModelState.IsValid)
            {
                var existingKhachHang = db.KhachHangs.Find(khachHang.MaKhachHang);
                if (existingKhachHang != null)
                {

                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.SDT = khachHang.SDT;
                    existingKhachHang.DiaChi = khachHang.DiaChi;
                    existingKhachHang.GioiTinh = khachHang.GioiTinh;
                    existingKhachHang.MatKhau = khachHang.MatKhau;
                    existingKhachHang.DiemTichLuy = khachHang.DiemTichLuy;
                    db.SaveChanges();
                    string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa khách hàng thành công ^^</h3></body></html>";
                    return Content(htmlContent, "text/html");
                }
                ViewBag.HangKhachHang = new SelectList(db.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang", khachHang.HangKhachHang);
                return View();

            }
            ViewBag.HangKhachHang = new SelectList(db.HangKhachHangs, "MaHangKhachHang", "TenHangKhachHang", khachHang.HangKhachHang);
            return View(khachHang);
        }

        [HttpPost]
        public ActionResult BlockUser(int? id)
        {
            var kh = db.KhachHangs.Find(id);

            if (kh != null)
            {
                kh.TrangThai = true;
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Chặn khách hàng thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult UndoUser(int? id)
        {
            var kh = db.KhachHangs.Find(id);

            if (kh != null)
            {
                kh.TrangThai = false;
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Hủy chặn thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return Json(new { success = true });
        }

    }
}
