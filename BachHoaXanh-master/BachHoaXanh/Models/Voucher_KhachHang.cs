//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BachHoaXanh.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Voucher_KhachHang
    {
        public int MaKhachHang { get; set; }
        public int MaVoucher { get; set; }
        public Nullable<int> SoLuong { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}