using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAWeb.Models
{
    public class GioHangItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string AnhSanPham { get; set; }
    }

    public class GioHang
    {
        public List<GioHangItem> Items { get; set; } = new List<GioHangItem>();

        public void AddItem(SanPham sanPham, int soLuong)
        {
            var item = Items.FirstOrDefault(i => i.MaSanPham == sanPham.MaSanPham);
            if (item == null)
            {
                Items.Add(new GioHangItem
                {
                    MaSanPham = sanPham.MaSanPham,
                    TenSanPham = sanPham.TenSanPham,
                    Gia = sanPham.Gia,
                    SoLuong = soLuong,
                    AnhSanPham = sanPham.AnhSanPham
                });
            }
            else
            {
                item.SoLuong += soLuong;
            }
        }

        public void RemoveItem(int maSanPham)
        {
            var item = Items.FirstOrDefault(i => i.MaSanPham == maSanPham);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.Gia * i.SoLuong);
        }
        public void Clear()
        {
            Items.Clear();
        }
    }
}