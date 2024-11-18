using DAWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DAWeb.Controllers
{

        public class ProductController : Controller
        {
            QlyBanGiayEntities db = new QlyBanGiayEntities();
            // GET: Product
            public ActionResult Index()
            {
                return View();
            }
            public ActionResult RecommendedItems(int categoryId)
            {
                // Lấy danh sách sản phẩm theo danh mục
                var recommendedProducts = db.SanPhams
              .Where(sp => sp.MaDanhMuc == categoryId)
              .OrderBy(x => Guid.NewGuid())
              .Take(3)
              .ToList();

                return PartialView("RecommendedItems", recommendedProducts); // Trả về PartialView
            }
            public ActionResult ChiTietSanPham(int id)
            {
                var sanPham = db.SanPhams.Find(id);
                if (sanPham == null)
                {
                    return HttpNotFound(); // Nếu không tìm thấy sản phẩm, trả về 404
                }
            return View(sanPham); //
            }
            public ActionResult SanPhamTheoDanhMuc(int categoryId)
            {

                // Lấy thông tin danh mục, để hiện tên danh mục theo từng danh mục
                var category = db.DanhMucs.Find(categoryId);
                if (category != null)
                {
                    ViewBag.CategoryName = category.TenDanhMuc; // Lưu tên danh mục vào ViewBag
                }

                // Lấy danh sách sản phẩm theo categoryId từ cơ sở dữ liệu
                var products = db.SanPhams.Where(sp => sp.MaDanhMuc == categoryId).ToList();
                return View(products);
            }


            public ActionResult PurchaseProduct(int productId, int quantity)
            {
                var product = db.SanPhams.Find(productId);
                if (product == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product not found");
                }

                if (product.SoLuongTon < quantity)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not enough stock available");
                }

                // Giảm số lượng tồn khi mua hàng
                product.SoLuongTon -= quantity;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                // Chuyển hướng tới trang xác nhận đơn hàng hoặc trang thông báo thành công
                return RedirectToAction("OrderConfirmation");
            }
      

    }
    
}