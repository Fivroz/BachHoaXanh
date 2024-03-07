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
    public class PhieuNhapKhoesController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: PhieuNhapKhoes
        public ActionResult Index()
        {
            var phieuNhapKhoes = db.PhieuNhapKhoes.Include(p => p.NhaCungCap).Include(p => p.NhanVien).Include(p => p.NhanVien1);
            return View(phieuNhapKhoes.ToList());
        }

        // GET: PhieuNhapKhoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuNhapKho);
        }

        // GET: PhieuNhapKhoes/Create
        public ActionResult Create()
        {
            ViewBag.MaNhaCC = new SelectList(db.NhaCungCaps, "MaNhaCC", "TenNhaCC");
            ViewBag.ProductList = db.SanPhams.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieuNhap,MaNhaCC")] PhieuNhapKho phieuNhapKho, List<int> productNames, List<int> quantities, List<decimal> prices)
        {
            if (ModelState.IsValid)
            {
                // Thêm phiếu nhập kho vào CSDL
                phieuNhapKho.NhanVienTao = Convert.ToInt32(Session["ID_ADMIN"]);
                phieuNhapKho.NhanVienNhap = null;
                phieuNhapKho.NgayTao = DateTime.Now;
                phieuNhapKho.TrangThai = "Đã xác nhận";
                phieuNhapKho.NgayNhap = null;
                db.PhieuNhapKhoes.Add(phieuNhapKho);
                db.SaveChanges();
           
                    // Thêm từng sản phẩm vào chi tiết phiếu nhập kho
                    for (int i = 0; i < productNames.Count; i++)
                    {
                        var productName = productNames[i];
                        var quantity = quantities[i];
                        var price = prices[i];

                        // Tạo chi tiết phiếu nhập kho
                        var chiTietPhieuNhapKho = new CTPhieuNhapKho()
                        {
                            MaPhieuNhap = phieuNhapKho.MaPhieuNhap,
                            MaSanPham = Convert.ToInt32(productName),
                            SoLuong = quantity,
                            GiaNhap = price
                        };

                        // Thêm chi tiết phiếu nhập kho vào CSDL
                        db.CTPhieuNhapKhoes.Add(chiTietPhieuNhapKho);
                        db.SaveChanges();
                    }
                return RedirectToAction("Index");
            }
            ViewBag.MaNhaCC = new SelectList(db.NhaCungCaps, "MaNhaCC", "TenNhaCC", phieuNhapKho.MaNhaCC);
            return View(phieuNhapKho);
        }
        // GET: PhieuNhapKhoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNhaCC = new SelectList(db.NhaCungCaps, "MaNhaCC", "TenNhaCC", phieuNhapKho.MaNhaCC);
            ViewBag.NhanVienTao = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", phieuNhapKho.NhanVienTao);
            ViewBag.NhanVienNhap = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", phieuNhapKho.NhanVienNhap);
            return View(phieuNhapKho);
        }

        // POST: PhieuNhapKhoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieuNhap,NhanVienTao,NhanVienNhap,MaNhaCC,NgayTao,NgayNhap,TrangThai")] PhieuNhapKho phieuNhapKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuNhapKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNhaCC = new SelectList(db.NhaCungCaps, "MaNhaCC", "TenNhaCC", phieuNhapKho.MaNhaCC);
            ViewBag.NhanVienTao = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", phieuNhapKho.NhanVienTao);
            ViewBag.NhanVienNhap = new SelectList(db.NhanViens, "MaNhanVien", "HoTen", phieuNhapKho.NhanVienNhap);
            return View(phieuNhapKho);
        }

        // GET: PhieuNhapKhoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuNhapKho);
        }

        // POST: PhieuNhapKhoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            db.PhieuNhapKhoes.Remove(phieuNhapKho);
            db.SaveChanges();
            return RedirectToAction("Index");
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
