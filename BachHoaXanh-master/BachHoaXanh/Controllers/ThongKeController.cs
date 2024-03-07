using BachHoaXanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BachHoaXanh.Controllers
{
    public class ThongKeController : Controller
    {
        private BACHHOAXANHEntities db = new BACHHOAXANHEntities();
        // GET: ThongKe
        public ActionResult ThongKeThang()
        {
            DateTime now = DateTime.Now;
            // Lấy tháng và năm từ ngày hiện tại
            int month = now.Month;
            int year = now.Year;

            // Lấy danh sách đơn hàng trong tháng và năm hiện tại
            var danhSachThang = db.DonDatHangs.Select(dh => new { Thang = dh.NgayDat.Month, Nam = dh.NgayDat.Year }).Distinct().ToList();

            ViewBag.DanhSachThang = new SelectList(danhSachThang, "Thang", "Thang");
            ViewBag.DanhSachNam = new SelectList(danhSachThang, "Nam", "Nam");

            return View();
        }
    }
}