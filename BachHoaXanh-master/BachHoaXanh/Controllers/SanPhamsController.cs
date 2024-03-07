using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BachHoaXanh.Models;
using System.IO;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using System.Text;

namespace BachHoaXanh.Controllers
{
    public class SanPhamsController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();

        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.DanhMuc).Include(s => s.ThuongHieu);
            return View(sanPhams.ToList());
        }
        public ActionResult Sp_ThitCa()
        {
            var ca = db.SanPhams.Where(s => s.DanhMuc.MaLoaiDanhMuc == 1).Include(s => s.ThuongHieu).Include(s => s.DanhMuc);
            return View(ca.Take(4).ToList());
        }
        public ActionResult SanPhamBanChay()
        {
            var sanPhamsBanChay = db.SanPhams
                .OrderByDescending(s => db.ChiTietDatHangs
                    .Where(ct => ct.MaSanPham == s.MaSanPham)
                    .Sum(ct => ct.SoLuong))
                .Take(4)
                .ToList();

            return View(sanPhamsBanChay);
        }

        public ActionResult Sp_RauCu()
        {
            var ca = db.SanPhams.Where(s => s.DanhMuc.MaLoaiDanhMuc == 2).Include(s => s.ThuongHieu).Include(s => s.DanhMuc);
            return View(ca.Take(4).ToList());
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSanPham,TenSanPham,GiaNiemYet,GiamGia,GiaBan,KhoiLuong,DungTich,ThanhPhan,CachDung,BaoQuan,LuuY,NoiSanXuat,MoTa,SoLuongTon,TrangThai,MaDanhMuc,MaThuongHieu")] SanPham sanPham, HttpPostedFileBase HinhSanPham)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sanPham.TenSanPham))
                    ModelState.AddModelError("TenSanPham", "Vui lòng nhập tên sản phẩm.");               
                if (sanPham.KhoiLuong < 0)
                    ModelState.AddModelError("KhoiLuong", "Vui lòng nhập khối lượng hợp lệ.");
                if (sanPham.DungTich < 0)
                    ModelState.AddModelError("DungTich", "Vui lòng nhập dung tích hợp lệ.");
                if (string.IsNullOrWhiteSpace(sanPham.ThanhPhan))
                    ModelState.AddModelError("ThanhPhan", "Vui lòng nhập thành phần.");
                if (string.IsNullOrWhiteSpace(sanPham.CachDung))
                    ModelState.AddModelError("CachDung", "Vui lòng nhập cách dùng.");
                if (string.IsNullOrWhiteSpace(sanPham.BaoQuan))
                    ModelState.AddModelError("BaoQuan", "Vui lòng nhập cách bảo quản.");
                if (string.IsNullOrWhiteSpace(sanPham.NoiSanXuat))
                    ModelState.AddModelError("NoiSanXuat", "Vui lòng nhập nơi sản xuất.");
                if (string.IsNullOrWhiteSpace(sanPham.MoTa))
                    ModelState.AddModelError("MoTa", "Vui lòng nhập mô tả.");
                if (!Regex.IsMatch(sanPham.TenSanPham, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                    ModelState.AddModelError("TenSanPham", "Tên sản phẩm không được chứa ký tự đặc biệt.");

                if (sanPham.TenSanPham.Length <= 8)
                    ModelState.AddModelError("TenSanPham", "Tên sản phẩm phải có độ dài lớn hơn 8 kí tự.");

                if (!Regex.IsMatch(sanPham.ThanhPhan, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                    ModelState.AddModelError("ThanhPhan", "Thành phần không được chứa ký tự đặc biệt.");

                if (!Regex.IsMatch(sanPham.CachDung, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                    ModelState.AddModelError("CachDung", "Cách dùng không được chứa ký tự đặc biệt.");

                if (!Regex.IsMatch(sanPham.BaoQuan, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                    ModelState.AddModelError("BaoQuan", "Bảo quản không được chứa ký tự đặc biệt.");

                if (!Regex.IsMatch(sanPham.MoTa, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                    ModelState.AddModelError("MoTa", "Mô tả không được chứa ký tự đặc biệt.");

                if (!Regex.IsMatch(sanPham.NoiSanXuat, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                    ModelState.AddModelError("NoiSanXuat", "Nơi sản xuất không được chứa ký tự đặc biệt.");

                if (sanPham.LuuY != null && !Regex.IsMatch(sanPham.LuuY, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                    ModelState.AddModelError("LuuY", "Lưu ý không được chứa ký tự đặc biệt.");
                var dm = db.SanPhams.FirstOrDefault(d => d.TenSanPham == sanPham.TenSanPham);
                if (dm != null)
                    ModelState.AddModelError(string.Empty, "Đã tồn tại tên này!");
                bool isNumeric = int.TryParse(sanPham.TenSanPham, out _);
                if (isNumeric)
                    ModelState.AddModelError(string.Empty, "Tên sản phẩm không được toàn số!");
                if (ModelState.IsValid)
                {
                    // Kiểm tra nếu người dùng đã chọn file hình ảnh
                    if (HinhSanPham != null && HinhSanPham.ContentLength > 0)
                    {
                        // Kiểm tra định dạng hình ảnh (vd: jpg, png, ...)
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(HinhSanPham.FileName);
                        if (!allowedExtensions.Contains(extension.ToLower()))
                        {
                            // Xử lý khi định dạng không hợp lệ
                            ModelState.AddModelError(string.Empty, "Định dạng hình ảnh không hợp lệ");
                            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
                            return View(sanPham);
                        }

                        // Kiểm tra kích thước tệp tin (vd: 5MB)
                        if (HinhSanPham.ContentLength > 5 * 1024 * 1024)
                        {
                            // Xử lý khi kích thước vượt quá giới hạn
                            ModelState.AddModelError(string.Empty, "Kích thước hình ảnh vượt quá giới hạn cho phép");
                            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
                            return View(sanPham);
                        }

                        // Lưu file vào thư mục trên server
                        string id = RemoveDiacriticsAndWhitespace(sanPham.TenSanPham); 
                        string _FileName = "hinh" + id + extension;
                        string path = Path.Combine(Server.MapPath("~/Styles/Image/SanPham"), _FileName);
                        HinhSanPham.SaveAs(path);

                        // Lưu tên tệp tin hình ảnh vào thuộc tính sanPham.HinhSanPham
                        sanPham.HinhSanPham = _FileName;

                    }
                    sanPham.SoLuongTon = 0;
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    ModelState.AddModelError(string.Empty, "Thêm thành công");
                    ModelState.AddModelError(string.Empty, "Hãy đóng modal để cập nhật trang");
                    ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                    ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
                    return View();
                }

                string htmlContent = "<html><body><h3 class=\"text-center\">Thêm sản phẩm thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
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


        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSanPham,TenSanPham,HinhSanPham,GiaNiemYet,GiamGia,GiaBan,KhoiLuong,DungTich,ThanhPhan,CachDung,BaoQuan,LuuY,NoiSanXuat,MoTa,SoLuongTon,TrangThai,MaDanhMuc,MaThuongHieu")] SanPham sanPham, HttpPostedFileBase HinhSanPham)
        {

            if (!Regex.IsMatch(sanPham.TenSanPham, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm không được chứa ký tự đặc biệt.");

            if (sanPham.TenSanPham.Length <= 8)
                ModelState.AddModelError("TenSanPham", "Tên sản phẩm phải có độ dài lớn hơn 8 kí tự.");

            if (!Regex.IsMatch(sanPham.ThanhPhan, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                ModelState.AddModelError("ThanhPhan", "Thành phần không được chứa ký tự đặc biệt.");

            if (!Regex.IsMatch(sanPham.CachDung, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                ModelState.AddModelError("CachDung", "Cách dùng không được chứa ký tự đặc biệt.");
            if (!Regex.IsMatch(sanPham.BaoQuan, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                ModelState.AddModelError("BaoQuan", "Bảo quản không được chứa ký tự đặc biệt.");


            if (!Regex.IsMatch(sanPham.MoTa, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                ModelState.AddModelError("MoTa", "Mô tả không được chứa ký tự đặc biệt.");

            if (!Regex.IsMatch(sanPham.NoiSanXuat, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9]+$"))
                ModelState.AddModelError("NoiSanXuat", "Nơi sản xuất không được chứa ký tự đặc biệt và số.");

            if (sanPham.LuuY != null && !Regex.IsMatch(sanPham.LuuY, @"^[a-zA-ZÀ-Ỷà-ỷỲỳÝỵỴỹỹỸĂăẮắẶặẰằẸẹẻỂểễÉéÈèẾỂểỘồỠỗỞừỬửỮữÝ\s,.//0-9()+\-*/.&#$@^!~]+$"))
                ModelState.AddModelError("LuuY", "Lưu ý không được chứa ký tự đặc biệt.");

            var dm = db.SanPhams.FirstOrDefault(d => d.TenSanPham == sanPham.TenSanPham);
            if (dm != null && dm.MaSanPham != sanPham.MaSanPham)
                ModelState.AddModelError(string.Empty, "Đã tồn tại tên này!");
            bool isNumeric = int.TryParse(sanPham.TenSanPham, out _);
            if (string.IsNullOrWhiteSpace(sanPham.TenSanPham))
                ModelState.AddModelError("TenSanPham", "Vui lòng nhập tên sản phẩm.");
            if (sanPham.KhoiLuong < 0)
                ModelState.AddModelError("KhoiLuong", "Vui lòng nhập khối lượng hợp lệ.");
            if (sanPham.DungTich < 0)
                ModelState.AddModelError("DungTich", "Vui lòng nhập dung tích hợp lệ.");
            if (string.IsNullOrWhiteSpace(sanPham.ThanhPhan))
                ModelState.AddModelError("ThanhPhan", "Vui lòng nhập thành phần.");
            if (string.IsNullOrWhiteSpace(sanPham.CachDung))
                ModelState.AddModelError("CachDung", "Vui lòng nhập cách dùng.");
            if (string.IsNullOrWhiteSpace(sanPham.BaoQuan))
                ModelState.AddModelError("BaoQuan", "Vui lòng nhập cách bảo quản.");
            if (string.IsNullOrWhiteSpace(sanPham.NoiSanXuat))
                ModelState.AddModelError("NoiSanXuat", "Vui lòng nhập nơi sản xuất.");
            if (string.IsNullOrWhiteSpace(sanPham.MoTa))
                ModelState.AddModelError("MoTa", "Vui lòng nhập mô tả.");
            if (isNumeric)
                ModelState.AddModelError(string.Empty, "Tên sản phẩm không được toàn số!");
        
            if (HinhSanPham != null && HinhSanPham.ContentLength > 0)
            {
                // Kiểm tra định dạng hình ảnh (vd: jpg, png, ...)
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(HinhSanPham.FileName);
                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError(string.Empty, "Định dạng hình ảnh không hợp lệ");
                    ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                    ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
                    return View(sanPham);
                }
                // Kiểm tra kích thước tệp tin (vd: 5MB)
                if (HinhSanPham.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "Kích thước hình ảnh vượt quá giới hạn cho phép");
                    ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                    ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
                    return View(sanPham);
                }
                string id = sanPham.MaSanPham.ToString();
                string directoryPath = Server.MapPath("~/Styles/Image/SanPham");
                string[] imageFiles = Directory.GetFiles(directoryPath, "hinh" + id + ".*");
                foreach (string file in imageFiles)
                    System.IO.File.Delete(file);
                string _FileName = "hinh" + id + extension;
                string path = Path.Combine(directoryPath, _FileName);
                HinhSanPham.SaveAs(path);
                // Cập nhật tên tệp hình ảnh trong thuộc tính sanPham.HinhSanPham
                sanPham.HinhSanPham = _FileName;
            }
            var trackedSanPham = db.SanPhams.Find(sanPham.MaSanPham);

            if (ModelState.IsValid)
            {
                if (sanPham.HinhSanPham != null)
                {
                    trackedSanPham.HinhSanPham = trackedSanPham.HinhSanPham;
                }
                trackedSanPham.TenSanPham = sanPham.TenSanPham;
                trackedSanPham.GiaNiemYet = sanPham.GiaNiemYet;
                trackedSanPham.GiamGia = sanPham.GiamGia;
                trackedSanPham.KhoiLuong = sanPham.KhoiLuong;
                trackedSanPham.DungTich = sanPham.DungTich;
                trackedSanPham.ThanhPhan = sanPham.ThanhPhan;
                trackedSanPham.CachDung = sanPham.CachDung;
                trackedSanPham.BaoQuan = sanPham.BaoQuan;
                trackedSanPham.LuuY = sanPham.LuuY;
                trackedSanPham.NoiSanXuat = sanPham.NoiSanXuat;
                trackedSanPham.MoTa = sanPham.MoTa;
                trackedSanPham.SoLuongTon = sanPham.SoLuongTon;
                trackedSanPham.TrangThai = sanPham.TrangThai;
                trackedSanPham.MaDanhMuc = sanPham.MaDanhMuc;
                trackedSanPham.MaThuongHieu = sanPham.MaThuongHieu;
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Sửa sản phẩm thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            ModelState.AddModelError(string.Empty, "Chỉnh sửa thất bại");
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieux, "MaThuongHieu", "TenThuongHieu", sanPham.MaThuongHieu);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);

            bool hasForeignKey = db.ChiTietDatHangs.Any(b => b.MaSanPham == id);
            if (hasForeignKey)
                // Nếu tồn tại khóa ngoại trong bảng khác, không được xóa
                ModelState.AddModelError(string.Empty, "Không thể xóa vì đã có khách hàng đặt sản phẩm!");         
            else
            {
                db.SanPhams.Remove(sanPham);
                db.SaveChanges();
                string htmlContent = "<html><body><h3 class=\"text-center\">Xóa sản phẩm thành công ^^</h3></body></html>";
                return Content(htmlContent, "text/html");
            }
            return View();
        }
    }
}
