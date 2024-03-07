using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using BachHoaXanh.Models;
using System.Text.RegularExpressions;
using System.Text;

namespace BachHoaXanh.Controllers
{
    public class LoaiDanhMucsController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: LoaiDanhMucs
        public ActionResult Index()
        {
            return View(db.LoaiDanhMucs.ToList());
        }     

        // GET: LoaiDanhMucs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiDanhMucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiDanhMuc,TenLoaiDanhMuc")] LoaiDanhMuc loaiDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(loaiDanhMuc.TenLoaiDanhMuc))
                ModelState.AddModelError("TenLoaiDanhMuc", "Vui lòng nhập tên Loại danh mục.");
            if (!Regex.IsMatch(loaiDanhMuc.TenLoaiDanhMuc, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
            {
                ModelState.AddModelError("TenLoaiDanhMuc", "Tên Loại danh mục không được chứa ký tự đặc biệt và số.");
            }

            if (loaiDanhMuc.TenLoaiDanhMuc.Length <= 8)
            {
                ModelState.AddModelError("TenLoaiDanhMuc", "Tên Loại danh mục phải có độ dài lớn hơn 8 kí tự.");
            }
            int totalCount = db.LoaiDanhMucs.Count();
            if (totalCount >= 20)
                ModelState.AddModelError(string.Empty, "Đã đạt số lượng Loại danh mục tối đa (20)!");
            var dm = db.LoaiDanhMucs.FirstOrDefault(d => d.TenLoaiDanhMuc == loaiDanhMuc.TenLoaiDanhMuc);
            if (dm != null)
                ModelState.AddModelError("TenLoaiDanhMuc", "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(loaiDanhMuc.TenLoaiDanhMuc, out _);
            if (isNumeric)
                ModelState.AddModelError("TenLoaiDanhMuc", "Tên Loại danh mục không được toàn số!");
            
            if (ModelState.IsValid)
            {
                db.LoaiDanhMucs.Add(loaiDanhMuc);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm loại danh mục thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(loaiDanhMuc);
        }
        // GET: LoaiDanhMucs/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDanhMuc loaiDanhMuc = db.LoaiDanhMucs.Find(id);
            if (loaiDanhMuc == null)
            {
                return HttpNotFound();
            }
            return View(loaiDanhMuc);
        }

        // POST: LoaiDanhMucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiDanhMuc,TenLoaiDanhMuc")] LoaiDanhMuc loaiDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(loaiDanhMuc.TenLoaiDanhMuc))
                ModelState.AddModelError("TenLoaiDanhMuc", "Vui lòng nhập tên Loại danh mục.");

            if (!Regex.IsMatch(loaiDanhMuc.TenLoaiDanhMuc, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenLoaiDanhMuc", "Tên Loại danh mục không được chứa ký tự đặc biệt và số.");

            if (loaiDanhMuc.TenLoaiDanhMuc.Length <= 8)
                ModelState.AddModelError("TenLoaiDanhMuc", "Tên Loại danh mục phải có độ dài lớn hơn 8 kí tự.");

            bool isNumeric = int.TryParse(loaiDanhMuc.TenLoaiDanhMuc, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên Loại danh mục không được toàn số!");
            var existingDanhMuc = db.LoaiDanhMucs.FirstOrDefault(d => d.TenLoaiDanhMuc == loaiDanhMuc.TenLoaiDanhMuc);
            if (existingDanhMuc != null && existingDanhMuc.MaLoaiDanhMuc != loaiDanhMuc.MaLoaiDanhMuc)
                ModelState.AddModelError(string.Empty, "Đã tồn tại tên này!");
            
            if (ModelState.IsValid)
            {
                var trackedDanhMuc = db.LoaiDanhMucs.Find(loaiDanhMuc.MaLoaiDanhMuc);
                if (trackedDanhMuc != null)
                {
                    trackedDanhMuc.TenLoaiDanhMuc = loaiDanhMuc.TenLoaiDanhMuc;
                    db.SaveChanges();
                    string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa loại danh mục thành công^^</h3></body></html>";
                    return Content(htmlContent, "text/html");
                }
                else
                    ModelState.AddModelError(string.Empty, "Không tìm thấy danh mục cần sửa!");
            }
            return View(loaiDanhMuc);
        }

        // GET: Test/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDanhMuc loaiDanhMuc = db.LoaiDanhMucs.Find(id);
            if (loaiDanhMuc == null)
            {
                return HttpNotFound();
            }
            return View(loaiDanhMuc);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            LoaiDanhMuc loaiDanhMuc = db.LoaiDanhMucs.Find(id);

            bool hasForeignKey = db.DanhMucs.Any(b => b.MaLoaiDanhMuc == id);
            if (hasForeignKey)
                // Nếu tồn tại khóa ngoại trong bảng khác, không được xóa
                ModelState.AddModelError(string.Empty, "Không thể xóa vì còn danh mục thuộc loại danh mục!");
            else
            {
                db.LoaiDanhMucs.Remove(loaiDanhMuc);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Xóa loại danh mục thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View();
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
