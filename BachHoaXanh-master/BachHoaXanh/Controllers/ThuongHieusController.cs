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
using System.IO;
using System.Text;

namespace BachHoaXanh.Controllers
{
    public class ThuongHieusController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: ThuongHieus
        public ActionResult Index()
        {
            return View(db.ThuongHieux.ToList());      
        }
        public ActionResult ChonThuongHieu()
        {
            
            var topSellingBrands = db.ThuongHieux.Take(12).ToList();
            return PartialView(topSellingBrands);
        }



        // GET: ThuongHieus/Create
        public ActionResult Create()
        {
            return View();
        }
        string RemoveDiacriticsAndWhitespace(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            Regex diacriticRegex = new Regex(@"(?<=\p{L})([\p{Mn}\p{Mc}]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string noDiacriticString = diacriticRegex.Replace(normalizedString, string.Empty);
            Regex whitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);
            string noWhitespaceString = whitespaceRegex.Replace(noDiacriticString, "");
            string replacedString = noWhitespaceString.Replace("đ", "d").Replace("Đ", "D");
            return replacedString.Normalize(NormalizationForm.FormC);
        }
        // POST: ThuongHieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThuongHieu,HinhThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu, HttpPostedFileBase HinhThuongHieu)
        {
            if (!Regex.IsMatch(thuongHieu.TenThuongHieu, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenThuongHieu", "Tên thương hiệu không được chứa ký tự đặc biệt.");
            if (thuongHieu.TenThuongHieu.Length <= 8)
                ModelState.AddModelError("TenThuongHieu", "Tên thương hiệu phải có độ dài lớn hơn 8 kí tự.");
            var th = db.ThuongHieux.FirstOrDefault(d => d.TenThuongHieu == thuongHieu.TenThuongHieu);
            if (th != null)
                ModelState.AddModelError(string.Empty, "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(thuongHieu.TenThuongHieu, out _);
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên Thương hiệu không được toàn số!");
            int totalCount = db.ThuongHieux.Count();
            if (totalCount >= 200)
                ModelState.AddModelError(string.Empty, "Đã đạt số lượng Thương hiệu tối đa (200)!");
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu người dùng đã chọn file hình ảnh
                if (HinhThuongHieu != null && HinhThuongHieu.ContentLength > 0)
                {
                    // Kiểm tra định dạng hình ảnh (vd: jpg, png, ...)
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(HinhThuongHieu.FileName);
                    if (!allowedExtensions.Contains(extension.ToLower()))
                    {
                        // Xử lý khi định dạng không hợp lệ
                        ModelState.AddModelError(string.Empty, "Định dạng hình ảnh không hợp lệ");
                        return View(thuongHieu);
                    }

                    // Kiểm tra kích thước tệp tin (vd: 5MB)
                    if (HinhThuongHieu.ContentLength > 5 * 1024 * 1024)
                    {
                        // Xử lý khi kích thước vượt quá giới hạn
                        ModelState.AddModelError(string.Empty, "Kích thước hình ảnh vượt quá giới hạn cho phép");
                        return View(thuongHieu);
                    }

                    // Lưu file vào thư mục trên server
                    string id = RemoveDiacriticsAndWhitespace(thuongHieu.TenThuongHieu);
                    string _FileName = "hinh" + id + extension;
                    string path = Path.Combine(Server.MapPath("~/Styles/Image/ThuongHieu"), _FileName);
                    HinhThuongHieu.SaveAs(path);

                    // Lưu tên tệp tin hình ảnh vào thuộc tính sanPham.HinhSanPham
                    thuongHieu.HinhThuongHieu = _FileName;

                }
                db.ThuongHieux.Add(thuongHieu);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm thương hiệu thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View(thuongHieu);
            }

        // GET: ThuongHieus/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThuongHieu,HinhThuongHieu,TenThuongHieu")] ThuongHieu thuongHieu, HttpPostedFileBase HinhThuongHieu)
        {
            if (!Regex.IsMatch(thuongHieu.TenThuongHieu, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenThuongHieu", "Tên thương hiệu không được chứa ký tự đặc biệt và số.");
            var existingThuongHieu = db.ThuongHieux.FirstOrDefault(d => d.TenThuongHieu == thuongHieu.TenThuongHieu);
            if (existingThuongHieu != null && existingThuongHieu.MaThuongHieu != thuongHieu.MaThuongHieu)
                ModelState.AddModelError("TenThuongHieu", "Đã tồn tại tên này!");

            bool isNumeric = int.TryParse(thuongHieu.TenThuongHieu, out _);
            if (isNumeric)
                ModelState.AddModelError("TenThuongHieu", "Tên Thương hiệu không được toàn số!");
            if (HinhThuongHieu != null && HinhThuongHieu.ContentLength > 0)
            {
                // Kiểm tra định dạng hình ảnh (vd: jpg, png, ...)
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(HinhThuongHieu.FileName);
                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError(string.Empty, "Định dạng hình ảnh không hợp lệ");
                    return View(thuongHieu);
                }

                // Kiểm tra kích thước tệp tin (vd: 5MB)
                if (HinhThuongHieu.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Kích thước hình ảnh vượt quá giới hạn cho phép");
                    return View(thuongHieu);
                }

                string id = thuongHieu.MaThuongHieu.ToString();

                string directoryPath = Server.MapPath("~/Styles/Image/ThuongHieu");
                string[] imageFiles = Directory.GetFiles(directoryPath, "hinh" + id + ".*");

                foreach (string file in imageFiles)
                {
                    // Xóa tệp hình ảnh cũ
                    System.IO.File.Delete(file);
                }

                string _FileName = "hinh" + id + extension;
                string path = Path.Combine(directoryPath, _FileName);

                HinhThuongHieu.SaveAs(path);

                // Cập nhật tên tệp hình ảnh trong thuộc tính thuongHieu.HinhThuongHieu
                thuongHieu.HinhThuongHieu = _FileName;
            }
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã chọn một tệp hình ảnh mới 
                var trackedThuongHieu = db.ThuongHieux.Find(thuongHieu.MaThuongHieu);
                if (trackedThuongHieu != null)
                {
                    // Cập nhật các trường thông tin khác
                    trackedThuongHieu.TenThuongHieu = thuongHieu.TenThuongHieu;
                    if (thuongHieu.HinhThuongHieu != null)
                        trackedThuongHieu.HinhThuongHieu = thuongHieu.HinhThuongHieu;
                    db.SaveChanges();

                    string htmlContent = "<html><body><h3 class=\"text-center\">Chỉnh sửa thương hiệu thành công ^^</h3></body></html>";
                    return Content(htmlContent, "text/html");
                }
            }

            ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra khi sửa!");
            return View(thuongHieu);
        }


        // GET: ThuongHieus/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            if (thuongHieu == null)
            {
                return HttpNotFound();
            }
            return View(thuongHieu);
        }

        // POST: ThuongHieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            ThuongHieu thuongHieu = db.ThuongHieux.Find(id);
            bool hasForeignKey = db.SanPhams.Any(b => b.MaDanhMuc == id);
            if (hasForeignKey)
                ModelState.AddModelError(string.Empty, "Không thể xóa vì còn sản phẩm thuộc danh mục!");
            else
            {
                db.ThuongHieux.Remove(thuongHieu);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Xóa thương hiệu thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View();
        }
    }
}
