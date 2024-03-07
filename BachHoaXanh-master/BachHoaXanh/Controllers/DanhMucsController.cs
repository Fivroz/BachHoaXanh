using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BachHoaXanh.Models;
using System.Text.RegularExpressions;

namespace BachHoaXanh.Controllers
{
    public class DanhMucsController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: DanhMucs
        public ActionResult Index()
        {
            var danhMucs = db.DanhMucs.Include(d => d.LoaiDanhMuc);
            return View(danhMucs.ToList());
        }

      
        // GET: DanhMucs/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiDanhMuc = new SelectList(db.LoaiDanhMucs, "MaLoaiDanhMuc", "TenLoaiDanhMuc");
            return View();
        }

        // POST: DanhMucs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDanhMuc,TenDanhMuc,MaLoaiDanhMuc")] DanhMuc danhMuc)
        {
            if (!Regex.IsMatch(danhMuc.TenDanhMuc, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.]+$"))
            {
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục không được chứa ký tự đặc biệt và số.");
            }
            var dm = db.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == danhMuc.TenDanhMuc);
            if (dm != null)
                ModelState.AddModelError("TenDanhMuc", "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(danhMuc.TenDanhMuc, out _);
            if (isNumeric)
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục không được toàn số!");
            int totalCount = db.DanhMucs.Count();
            if (totalCount >= 200)
                ModelState.AddModelError(string.Empty, "Đã đạt số lượng Danh mục tối đa (200)!");
            if (ModelState.IsValid)
            {
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();              
                ViewBag.MaLoaiDanhMuc = new SelectList(db.LoaiDanhMucs, "MaLoaiDanhMuc", "TenLoaiDanhMuc", danhMuc.MaLoaiDanhMuc);
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm danh mục thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }

            ViewBag.MaLoaiDanhMuc = new SelectList(db.LoaiDanhMucs, "MaLoaiDanhMuc", "TenLoaiDanhMuc", danhMuc.MaLoaiDanhMuc);
            return View(danhMuc);
        }

        // GET: DanhMucs/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiDanhMuc = new SelectList(db.LoaiDanhMucs, "MaLoaiDanhMuc", "TenLoaiDanhMuc", danhMuc.MaLoaiDanhMuc);
            return View(danhMuc);
        }

        // POST: DanhMucs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDanhMuc,TenDanhMuc,MaLoaiDanhMuc")] DanhMuc danhMuc)
        {
            if (!Regex.IsMatch(danhMuc.TenDanhMuc, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
            {
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục không được chứa ký tự đặc biệt.");
            }
            var existingDanhMuc = db.DanhMucs.FirstOrDefault(d => d.TenDanhMuc == danhMuc.TenDanhMuc && d.MaDanhMuc != danhMuc.MaDanhMuc && d.MaLoaiDanhMuc == danhMuc.MaLoaiDanhMuc);

            if (existingDanhMuc != null)
                ModelState.AddModelError("TenDanhMuc", "Đã tồn tại tên này!");

            bool isNumeric = int.TryParse(danhMuc.TenDanhMuc, out _);
            if (isNumeric)
                ModelState.AddModelError("TenDanhMuc", "Tên danh mục không được toàn số!");

            if (ModelState.IsValid)
            {   var trackedDanhMuc = db.DanhMucs.Find(danhMuc.MaDanhMuc);
            if (trackedDanhMuc != null)
            {
                trackedDanhMuc.TenDanhMuc = danhMuc.TenDanhMuc;
                trackedDanhMuc.MaLoaiDanhMuc = danhMuc.MaLoaiDanhMuc;
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa danh mục thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html"); 
             }
               else
                ModelState.AddModelError(string.Empty, "Không tìm thấy danh mục cần sửa!"); 
            }
            ViewBag.MaLoaiDanhMuc = new SelectList(db.LoaiDanhMucs, "MaLoaiDanhMuc", "TenLoaiDanhMuc", danhMuc.MaLoaiDanhMuc);
            return View(danhMuc);
        }

        // GET: DanhMucs/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMuc danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            return View(danhMuc);
        }

        // POST: DanhMucs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            DanhMuc danhMuc = db.DanhMucs.Find(id);

            bool hasForeignKey = db.SanPhams.Any(b => b.MaDanhMuc == id);
            if (hasForeignKey)
            {
                // Nếu tồn tại khóa ngoại trong bảng khác, không được xóa
                ModelState.AddModelError(string.Empty, "Không thể xóa vì còn sản phẩm thuộc danh mục!");
            }
            else
            {
                db.DanhMucs.Remove(danhMuc);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Xóa danh mục thành công^^</h3></body></html>";
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
