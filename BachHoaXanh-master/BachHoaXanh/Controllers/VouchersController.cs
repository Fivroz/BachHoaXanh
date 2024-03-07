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
    public class VouchersController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: Vouchers
        public ActionResult Index()
        {
            return View(db.Vouchers.ToList());
        }

        // GET: Vouchers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaVoucher,MaCode,GiaTri,GiaTriType,NgayHetHan,TrangThai,MoTa")] Voucher voucher, string GiaTriType)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var code = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            while (db.Vouchers.Any(v => v.MaCode == code))
            {
                code = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            if (voucher.GiaTriType == "phantram" && (voucher.GiaTri <= 0 || voucher.GiaTri >= 100))
            {
                ModelState.AddModelError("GiaTri", "Trị giá phải lớn hơn 0 và nhỏ hơn 100 khi là %");
            }
            else if (voucher.GiaTriType == "sotien" && voucher.GiaTri <= 10000)
            {
                ModelState.AddModelError("GiaTri", "Trị giá phải lớn hơn 10.000 khi là VNĐ");
            }

            if (!Regex.IsMatch(voucher.MoTa, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9%]+$"))
            {
                ModelState.AddModelError("MoTa", "Mô tả không được chứa ký tự đặc biệt.");
            }
            if (ModelState.IsValid)
            {
                voucher.GiaTriType = "sotien";
                if (GiaTriType == "phantram")
                {
                    voucher.GiaTri /= 100;
                    voucher.GiaTriType = "phantram";
                }
                voucher.TrangThai = "Đang mở";
                voucher.MaCode = code;
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm voucher thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(voucher);
        }



        // GET: Vouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaVoucher,MaCode,GiaTri,GiaTriType,NgayHetHan,TrangThai,MoTa")] Voucher voucher)
        {
            if (voucher.GiaTriType == "phantram" && (voucher.GiaTri <= 0 || voucher.GiaTri >= 100))
            {
                ModelState.AddModelError("GiaTri", "Trị giá phải lớn hơn 0 và nhỏ hơn 100 khi là %");
            }
            else if (voucher.GiaTriType == "sotien" && voucher.GiaTri <= 10000)
            {
                ModelState.AddModelError("GiaTri", "Trị giá phải lớn hơn 10.000 khi là VNĐ");
            }
            if (!Regex.IsMatch(voucher.MoTa, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9%]+$"))
            {
                ModelState.AddModelError("MoTa", "Mô tả không được chứa ký tự đặc biệt.");
            }
            if (voucher.GiaTriType == "phantram" && voucher.GiaTri < 1)
                voucher.GiaTri *= 100;
            var trackedVoucher = db.Vouchers.Find(voucher.MaVoucher);
            if (ModelState.IsValid)
            {
                if (voucher.GiaTriType == "phantram")
                {
                    trackedVoucher.GiaTri = voucher.GiaTri/100;
                }
                if (voucher.NgayHetHan != null)
                {
                    trackedVoucher.NgayHetHan = voucher.NgayHetHan;
                }
                trackedVoucher.MoTa = voucher.MoTa;
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa voucher thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(voucher);
        }
        [HttpPost]
        public ActionResult BlockVoucher(int? id)
        {
            var kh = db.Vouchers.Find(id);

            if (kh != null)
            {
                kh.TrangThai = "Đã khóa";
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Khóa voucher thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UndoVoucher(int? id)
        {
            var kh = db.Vouchers.Find(id);

            if (kh != null)
            {
                kh.TrangThai = "Đang mở";
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Hủy chặn voucher thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateStatusVouchers()
        {
            DateTime ngayHienTai = DateTime.Now;

            var danhSachVoucherHetHan = db.Vouchers.Where(v => v.NgayHetHan < ngayHienTai && v.TrangThai != "Hết hạn").ToList();

            foreach (var voucher in danhSachVoucherHetHan)
            {
                voucher.TrangThai = "Hết hạn";
            }

            var danhSachVoucherConHan = db.Vouchers.Where(v => v.NgayHetHan >= ngayHienTai && v.TrangThai != "Đang mở" && v.TrangThai != "Đã khóa").ToList();

            foreach (var voucher in danhSachVoucherConHan)
            {
                voucher.TrangThai = "Đang mở";
            }

            db.SaveChanges();
            return RedirectToAction("Index"); // Hoặc chuyển hướng đến view hiển thị danh sách voucher
        }

    }
}
