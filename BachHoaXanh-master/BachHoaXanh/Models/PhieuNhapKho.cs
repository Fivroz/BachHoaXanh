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
    
    public partial class PhieuNhapKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhapKho()
        {
            this.CTPhieuNhapKhoes = new HashSet<CTPhieuNhapKho>();
        }
    
        public int MaPhieuNhap { get; set; }
        public int NhanVienTao { get; set; }
        public Nullable<int> NhanVienNhap { get; set; }
        public Nullable<int> MaNhaCC { get; set; }
        public System.DateTime NgayTao { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public string TrangThai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTPhieuNhapKho> CTPhieuNhapKhoes { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        public virtual NhanVien NhanVien1 { get; set; }
    }
}