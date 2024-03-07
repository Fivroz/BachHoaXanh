using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BachHoaXanh.Models
{
    public class Cart
    {
        BACHHOAXANHEntities database = new BACHHOAXANHEntities();
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string AnhSanPham { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public int KhoiLuong { get; set; }
        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }
        public int TKhoiLuong()
        {
            return SoLuong * KhoiLuong;
        }
        public Cart(int MaSP)
        {
            this.MaSanPham = MaSP;
            //Tìm sách trong CSDL có mã id cần và gán cho mặt hàng được mua
            var sp = database.SanPhams.Single(s => s.MaSanPham == this.MaSanPham);
            this.TenSanPham = sp.TenSanPham;
            this.AnhSanPham = sp.HinhSanPham;
            this.KhoiLuong = (int) sp.KhoiLuong;
            this.DonGia = double.Parse(((double)sp.GiaNiemYet - ((double)sp.GiaNiemYet * (double)sp.GiamGia)).ToString());
            this.SoLuong = 1; //Số lượng mua ban đầu của một mặt hàng là 1 (cho lần click đầu)
        }
    }
}