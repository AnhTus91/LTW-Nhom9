//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }
    
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public int SoLuongTon { get; set; }
        public string AnhSanPham { get; set; }
        public Nullable<int> MaDanhMuc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DanhMuc DanhMuc { get; set; }
    }
}
