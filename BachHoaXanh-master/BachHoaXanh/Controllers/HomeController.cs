using BachHoaXanh.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BachHoaXanh.Controllers
{
    public class HomeController : Controller
    {
        BACHHOAXANHEntities databases = new BACHHOAXANHEntities();
        public ActionResult TrangChu()
        {
            var dsSanPham = databases.SanPhams.Where(sp => sp.TrangThai != "Hủy bán").ToList();
            return View(dsSanPham);
        }
        public ActionResult Index()
        {
            var dsSanPham = databases.SanPhams.Where(sp => sp.TrangThai != "Hủy bán").ToList();
            return View(dsSanPham);
        }
        public ActionResult SanPhamTheoDM(int id)
        {
            var dsTheoDM = databases.SanPhams.Where(sp => sp.MaDanhMuc == id).ToList();
            var relatedBrands = dsTheoDM.Select(th => th.ThuongHieu).Distinct().ToList();

            ViewBag.RelatedBrands = relatedBrands;
            return View("SearchByName", "_Layout", dsTheoDM);
        }

        [HttpGet]
        public ActionResult SearchByName(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction("TrangChu");
            }

            @ViewBag.Keyword = searchString;
            var ds = databases.SanPhams.Where(sp => sp.TenSanPham.ToUpper().Contains(searchString)).ToList();
            var relatedBrands = ds.Select(th => th.ThuongHieu).Distinct().ToList();

            ViewBag.RelatedBrands = relatedBrands;
            return View(ds);
        }


        public ActionResult LayDanhMuc()
        {
            var dsDanhMuc = databases.LoaiDanhMucs.ToList();
            return PartialView(dsDanhMuc);
        }
        public ActionResult LayDanhMucCon(int maDanhMuc)
        {
            var danhMuc = databases.DanhMucs.FirstOrDefault(dm => dm.MaDanhMuc == maDanhMuc);
            var dsDanhMucCon = danhMuc.TenDanhMuc.ToList();
            return PartialView(dsDanhMucCon);
        }

        public ActionResult LayDSThuongHieu()
        {
            var dsThuongHieu = databases.ThuongHieux.ToList();
            return PartialView(dsThuongHieu);
        }
        public ActionResult SanPhamTheoTH(int id)
        {
            var dsTheoTH = databases.SanPhams.Where(sp => sp.MaThuongHieu == id).ToList();
            return View("SearchByName", "_Layout", dsTheoTH);

        }
        public ActionResult CTSanPham(int id)
        {
            var ctSanPham = databases.SanPhams.FirstOrDefault(s => s.MaSanPham == id);
            return View(ctSanPham);
        }
        [HttpGet]
        public ActionResult Login()
        {

            if (Session["ID_ADMIN"] != null || Session["ID_USER"] != null)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(KhachHang kh, NhanVien nv)// chuyền vô bảng khách hàng viết tắt là kh thôi 
        {
            if (string.IsNullOrEmpty(kh.SDT))
                ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
            if (string.IsNullOrEmpty(kh.MatKhau))
                ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
            // kiểm tra
            var check_user = databases.KhachHangs.Where(s => s.SDT == kh.SDT && s.MatKhau == kh.MatKhau).FirstOrDefault();
            var check_admin = databases.NhanViens.Where(s => s.SDT == nv.SDT && s.MatKhau == nv.MatKhau).FirstOrDefault();

            if (check_admin == null && check_user == null) // nếu khác thì báo lỗi (bên trang đăng nhập có hiện lỗi ròi nhưng trong trường hợp nhập sai này thì thấy chưa báo lỗi có j xem lại giúp)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng");
                return View();
            }
            // lưu vào Session
            else
            {

                // check user
                if (check_user != null)
                {
                    if (check_user.TrangThai == true)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản này đã bị chặn");
                        return View();
                    }
                    else
                    {
                        KhachHang data = databases.KhachHangs.Where(s => s.SDT == kh.SDT && s.MatKhau == nv.MatKhau).FirstOrDefault();

                        Session["User"] = data;
                        Session["ID_USER"] = data.MaKhachHang;
                        Session["NAME_USER"] = data.HoTen;
                        return RedirectToAction("TrangChu", "Home");
                    }
                }
                // check admin
                if (check_admin != null)
                {

                    NhanVien data = databases.NhanViens.Where(s => s.SDT == nv.SDT && s.MatKhau == nv.MatKhau).FirstOrDefault();
                    Session["ID_ADMIN"] = data.MaNhanVien;
                    Session["ROLE_ADMIN"] = data.VaiTro;
                    Session["NAME_ADMIN"] = data.HoTen;
                    if (Session["ID_ADMIN"] != null && data.TrangThai == false)
                    {
                        if (Session["ROLE_ADMIN"] != null)
                        {
                            if ((int)(short)Session["ROLE_ADMIN"] == 1)
                            {
                                return RedirectToAction("NVIT", "ADMIN");
                            }
                            if ((int)(short)Session["ROLE_ADMIN"] == 2)
                            {
                                return RedirectToAction("NVKD", "ADMIN");
                            }
                            if ((int)(short)Session["ROLE_ADMIN"] == 3)
                            {
                                return RedirectToAction("NVGH", "ADMIN");
                            }
                            if ((int)(short)Session["ROLE_ADMIN"] == 4)
                            {
                                return RedirectToAction("NVK", "ADMIN");
                            }
                            ModelState.AddModelError(string.Empty, "Tài khoản này đã bị chặn");
                            return View();//sài thì về lại đăng nhập
                        }

                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        Session.Clear();
                        Session.RemoveAll();
                        Session.Abandon();
                        return RedirectToAction("Login");
                    }
                }
                return View("Login");//sài thì về lại đăng nhập
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Rigister()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}