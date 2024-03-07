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
    public class NhanViensController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: NhanViens
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Include(n => n.VaiTroNhanVien);
            return View(nhanViens.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public ActionResult Create()
        {
            ViewBag.VaiTro = new SelectList(db.VaiTroNhanViens, "MaVaiTro", "TenVaiTro");
            return View();
        }

        // POST: NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNhanVien,HoTen,GioiTinh,SDT,DiaChi,MatKhau,VaiTro,TrangThai")] NhanVien nhanVien)
        {
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
                ModelState.AddModelError("HoTen", "Vui lòng nhập họ tên.");
            if (string.IsNullOrEmpty(nhanVien.SDT))
                ModelState.AddModelError("SDT", "Vui lòng nhập tài khoản.");
            if (string.IsNullOrEmpty(nhanVien.MatKhau))
                ModelState.AddModelError("MatKhau", "Vui lòng nhập mật khẩu.");
            if (string.IsNullOrEmpty(nhanVien.DiaChi))
                ModelState.AddModelError("DiaChi", "Vui lòng nhập địa chỉ.");

            if (!Regex.IsMatch(nhanVien.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s]+$"))
                ModelState.AddModelError("HoTen", "Tên nhân viên không được chứa ký tự đặc biệt và số.");

            if (nhanVien.HoTen.Length <= 8)
                ModelState.AddModelError("HoTen", "Tên nhân viên phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(nhanVien.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được chứa ký tự đặc biệt và số.");

            if (nhanVien.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");


            if (!Regex.IsMatch(nhanVien.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDT", "Số điện thoại không đúng định dạng");

            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VaiTro = new SelectList(db.VaiTroNhanViens, "MaVaiTro", "TenVaiTro", nhanVien.VaiTro);
            return View(nhanVien);
        }

        // GET: NhanViens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.VaiTro = new SelectList(db.VaiTroNhanViens, "MaVaiTro", "TenVaiTro", nhanVien.VaiTro);
            return View(nhanVien);
        }

        // POST: NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNhanVien,HoTen,GioiTinh,SDT,DiaChi,MatKhau,VaiTro,TrangThai")] NhanVien nhanVien)
        {
            bool isNumeric = int.TryParse(nhanVien.HoTen, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên nhân viên không được toàn số!");

            if (string.IsNullOrEmpty(nhanVien.MatKhau))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập mật khẩu!");

            if (string.IsNullOrEmpty(nhanVien.DiaChi))
                ModelState.AddModelError(string.Empty, "Vui lòng nhập địa chỉ!");

            if (!Regex.IsMatch(nhanVien.HoTen, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s]+$"))
                ModelState.AddModelError("HoTen", "Tên nhân viên không được chứa ký tự đặc biệt và số.");

            if (nhanVien.HoTen.Length <= 8)
                ModelState.AddModelError("HoTen", "Tên nhân viên phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(nhanVien.DiaChi, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("DiaChi", "Địa chỉ không được chứa ký tự đặc biệt và số.");

            if (nhanVien.DiaChi.Length <= 8)
                ModelState.AddModelError("DiaChi", "Địa chỉ phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(nhanVien.SDT, @"^\d{10}$"))
                ModelState.AddModelError("SDT", "Số điện thoại không đúng định dạng");
            if (ModelState.IsValid)
            {
                var trackedNhanVien = db.NhanViens.Find(nhanVien.MaNhanVien);
                if (trackedNhanVien != null)
                {
                    trackedNhanVien.HoTen = nhanVien.HoTen;
                    trackedNhanVien.GioiTinh = nhanVien.GioiTinh;
                    trackedNhanVien.SDT = nhanVien.SDT;
                    trackedNhanVien.DiaChi = nhanVien.DiaChi;
                    trackedNhanVien.MatKhau = nhanVien.MatKhau;
                    trackedNhanVien.VaiTro = nhanVien.VaiTro;
                    trackedNhanVien.TrangThai = nhanVien.TrangThai;

                    db.SaveChanges();
                    string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa nhân viên thành công ^^</h3></body></html>";
                    return Content(htmlContent, "text/html");
                }
                else
                    ModelState.AddModelError(string.Empty, "Không tìm thấy nhân viên cần sửa!");
            }
            ViewBag.VaiTro = new SelectList(db.VaiTroNhanViens, "MaVaiTro", "TenVaiTro", nhanVien.VaiTro);
            return View(nhanVien);
        }
    }
}
