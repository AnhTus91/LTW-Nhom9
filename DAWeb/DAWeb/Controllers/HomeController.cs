using DAWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWeb.Controllers
{
    public class HomeController : Controller
    {
        private QlyWebBanGiayEntities db = new QlyWebBanGiayEntities();
        public ActionResult Index()
        {
            var sanPhams = db.SanPham.OrderByDescending(sp => sp.MaSanPham).ToList();
            return View(sanPhams);
        }
        public ActionResult Search(string keyword)
        {
            // Kiểm tra nếu keyword có giá trị
            if (!string.IsNullOrEmpty(keyword))
            {
                // Tìm kiếm sản phẩm theo tên
                var sanPhams = db.SanPham
                                 .Where(sp => sp.TenSanPham.Contains(keyword))
                                 .OrderByDescending(sp => sp.MaSanPham)
                                 .ToList();
                return View("Index", sanPhams); // Trả về danh sách sản phẩm tìm được
            }

            // Nếu không có từ khóa, trả về toàn bộ sản phẩm
            return RedirectToAction("Index");
        }
        //public ActionResult ChiTietSanPham(int id)
        //{
        //    var sanPham = db.SanPham.Find(id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound(); // Nếu không tìm thấy sản phẩm, trả về 404
        //    }

        //    return View(sanPham); //
        //}
    }
}