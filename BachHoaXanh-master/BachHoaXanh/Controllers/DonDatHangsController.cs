using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BachHoaXanh.Models;

namespace BachHoaXanh.Controllers
{
    public class DonDatHangsController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: DonDatHangs
        public ActionResult Index()
        {
            var donDatHangs = db.DonDatHangs.Include(d => d.KhachHang).Include(d => d.NhanVien).Include(d => d.PhiGiaoHang1).Include(d => d.PhuongThucThanhToan1);
            return View(donDatHangs.ToList());
        }


        // GET: DonDatHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "HoTen", donDatHang.MaKhachHang);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", donDatHang.MaNhanVien);
            ViewBag.PhiGiaoHang = new SelectList(db.PhiGiaoHangs, "MaPhi", "MaPhi", donDatHang.PhiGiaoHang);
            ViewBag.PhuongThucThanhToan = new SelectList(db.PhuongThucThanhToans, "MaPhuongThuc", "TenPhuongThuc", donDatHang.PhuongThucThanhToan);
            return View(donDatHang);
        }

        // POST: DonDatHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,NgayDat,MaKhachHang,MaNhanVien,PhiGiaoHang,TongTien,PhuongThucThanhToan,TrangThaiDonHang,TrangThaiDatHang")] DonDatHang donDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donDatHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKhachHang = new SelectList(db.KhachHangs, "MaKhachHang", "HoTen", donDatHang.MaKhachHang);
            ViewBag.MaNhanVien = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", donDatHang.MaNhanVien);
            ViewBag.PhiGiaoHang = new SelectList(db.PhiGiaoHangs, "MaPhi", "MaPhi", donDatHang.PhiGiaoHang);
            ViewBag.PhuongThucThanhToan = new SelectList(db.PhuongThucThanhToans, "MaPhuongThuc", "TenPhuongThuc", donDatHang.PhuongThucThanhToan);
            return View(donDatHang);
        }
        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            // Lấy dữ liệu đơn hàng từ CSDL dựa trên orderId
            var order = db.DonDatHangs.Find(orderId);
            if (order != null)
            {
                // Cập nhật trạng thái của đơn hàng thành "Hủy đơn hàng"
                order.TrangThaiDonHang = "Hủy đơn hàng";
                db.SaveChanges();
                ModelState.AddModelError(string.Empty, "Hủy thành công!");
                return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
            }

            return Json(new { success = true });
        }
        public ActionResult IndexGH()
        {
            var donDatHangs = db.DonDatHangs.Include(d => d.KhachHang).Include(d => d.NhanVien).Include(d => d.PhiGiaoHang1).Include(d => d.PhuongThucThanhToan1).Where(s=> s.TrangThaiDonHang == "Đã xác nhận");
            return View(donDatHangs.ToList());
        }
        public ActionResult IndexNhanHang(int? id)
        {
            var donDatHangs = db.DonDatHangs.Where(s => s.MaNhanVien == id).ToList();
            return View(donDatHangs);
        }
        [HttpPost]
        public ActionResult NhanDon(int? id)
        {
            // Tìm đơn đặt hàng theo mã đơn hàng
            var donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                // Xử lý khi không tìm thấy đơn đặt hàng (nếu cần)
                return HttpNotFound();
            }

            // Cập nhật mã nhân viên của đơn đặt hàng
            donDatHang.MaNhanVien = (int)Session["ID_ADMIN"];
            donDatHang.TrangThaiDonHang = "Đang giao hàng";
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Trả về kết quả về mã nhân viên để cập nhật trong giao diện
            return RedirectToAction("IndexGH");
        }
        [HttpPost]
        public ActionResult HoanThanhDon(int? id)
        {
            // Tìm đơn đặt hàng theo mã đơn hàng
            var donDatHang = db.DonDatHangs.Find(id);
            if (donDatHang == null)
            {
                // Xử lý khi không tìm thấy đơn đặt hàng (nếu cần)
                return HttpNotFound();
            }

            // Cập nhật mã nhân viên của đơn đặt hàng
            donDatHang.TrangThaiDonHang = "Đã giao hàng";
            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            // Trả về kết quả về mã nhân viên để cập nhật trong giao diện
            return RedirectToAction("IndexNhanHang");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
