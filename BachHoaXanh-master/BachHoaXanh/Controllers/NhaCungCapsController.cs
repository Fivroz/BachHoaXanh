using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using BachHoaXanh.Models;

namespace BachHoaXanh.Controllers
{
    public class NhaCungCapsController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: NhaCungCaps
        public ActionResult Index()
        {
            return View(db.NhaCungCaps.ToList());
        }

        // GET: NhaCungCaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhaCC,TenNhaCC,DiaChi,SDT")] NhaCungCap nhaCungCap)
        {
            if (string.IsNullOrWhiteSpace(nhaCungCap.TenNhaCC))
                ModelState.AddModelError("TenNhaCC", "Vui lòng nhập tên nhà cung cấp.");
            if (!Regex.IsMatch(nhaCungCap.TenNhaCC, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp không được chứa ký tự đặc biệt.");
            if (nhaCungCap.TenNhaCC.Length <= 8)
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp phải có độ dài lớn hơn 8 kí tự.");
            var dm = db.NhaCungCaps.FirstOrDefault(d => d.TenNhaCC == nhaCungCap.TenNhaCC);
            if (dm != null)
                ModelState.AddModelError("TenNhaCC", "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(nhaCungCap.TenNhaCC, out _);
            if (isNumeric)
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp không được toàn số!");

            if (!Regex.IsMatch(nhaCungCap.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDT", "Số điện thoại không đúng định dạng");

            if (string.IsNullOrWhiteSpace(nhaCungCap.DiaChi))
                ModelState.AddModelError("DiaChi", "Vui lòng nhập địa chỉ.");
            if (!Regex.IsMatch(nhaCungCap.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Tên địa chỉ không được chứa ký tự đặc biệt.");
            if (nhaCungCap.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Tên địa chỉ phải có độ dài lớn hơn 8 kí tự.");
            isNumeric = int.TryParse(nhaCungCap.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError("DiaChi", "Tên địa chỉ không được toàn số!");


            if (ModelState.IsValid)
            {
                db.NhaCungCaps.Add(nhaCungCap);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm nhà cung cấp thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }

            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhaCC,TenNhaCC,DiaChi,SDT")] NhaCungCap nhaCungCap)
        {
            if (string.IsNullOrWhiteSpace(nhaCungCap.TenNhaCC))
                ModelState.AddModelError("TenNhaCC", "Vui lòng nhập tên nhà cung cấp.");
            if (!Regex.IsMatch(nhaCungCap.TenNhaCC, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp không được chứa ký tự đặc biệt.");
            if (nhaCungCap.TenNhaCC.Length <= 8)
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp phải có độ dài lớn hơn 8 kí tự.");
            var existingNCC = db.NhaCungCaps.FirstOrDefault(d => d.TenNhaCC == nhaCungCap.TenNhaCC);
            if (existingNCC != null && existingNCC.MaNhaCC != nhaCungCap.MaNhaCC)
                ModelState.AddModelError(string.Empty, "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(nhaCungCap.TenNhaCC, out _);
            if (isNumeric)
                ModelState.AddModelError("TenNhaCC", "Tên nhà cung cấp không được toàn số!");

            if (!Regex.IsMatch(nhaCungCap.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDT", "Số điện thoại không đúng định dạng");

            if (string.IsNullOrWhiteSpace(nhaCungCap.DiaChi))
                ModelState.AddModelError("DiaChi", "Vui lòng nhập địa chỉ.");
            if (!Regex.IsMatch(nhaCungCap.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Tên địa chỉ không được chứa ký tự đặc biệt.");
            if (nhaCungCap.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Tên địa chỉ phải có độ dài lớn hơn 8 kí tự.");
            isNumeric = int.TryParse(nhaCungCap.DiaChi, out _);
            if (isNumeric)
                ModelState.AddModelError("DiaChi", "Tên địa chỉ không được toàn số!");

            var trackedNCC = db.NhaCungCaps.Find(nhaCungCap.MaNhaCC);
            if (ModelState.IsValid)
            {
                if(trackedNCC != null)
                {
                    trackedNCC.SDT = nhaCungCap.SDT;
                    trackedNCC.TenNhaCC = nhaCungCap.TenNhaCC;
                    trackedNCC.DiaChi = nhaCungCap.DiaChi;
                }
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Cập nhật nhà cung cấp thành công^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhaCungCap nhaCungCap = db.NhaCungCaps.Find(id);

            bool hasForeignKey = db.PhieuNhapKhoes.Any(b => b.MaNhaCC == id);
            if (hasForeignKey)
                ModelState.AddModelError(string.Empty, "Không thể xóa vì nhà cung cấp này có phiếu nhập kho!");
            else
            {
                db.NhaCungCaps.Remove(nhaCungCap);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Xóa nhà cung cấp thành công^^</h3></body></html>";
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
