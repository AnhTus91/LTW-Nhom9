using System;
using System.Linq;
using System.Web.Mvc;
using DAWeb.Models;

namespace DAWeb.Controllers
{

    public class CartController : Controller
    {
        QlyWebBanGiayEntities db = new QlyWebBanGiayEntities();

        // Retrieve the cart from session
        private GioHang GetCart()
        {
            var cart = Session["Cart"] as GioHang;
            if (cart == null)
            {
                cart = new GioHang();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // Add item to cart
        public ActionResult AddToCart(int productId, int quantity)
        {
            if (Session["userLogin"] == null)
            {
                TempData["Message"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.";
                return RedirectToAction("Login", "User");
            }

            SanPham product = db.SanPham.Find(productId);
            if (product != null)
            {
                GetCart().AddItem(product, quantity);
            }
            return RedirectToAction("Index");
        }

        // View the cart
        public ActionResult Index()
        {
            GioHang cart = GetCart();
            ViewBag.CartItemCount = cart.Items.Sum(i => i.SoLuong); // Tính tổng số lượng sản phẩm
            return View(cart);
        }

        // Remove item from cart
        public ActionResult RemoveFromCart(int productId)
        {
            GetCart().RemoveItem(productId);
            return RedirectToAction("Index");
        }

        // Get total price
        public ActionResult GetTotalPrice()
        {
            var totalPrice = GetCart().GetTotalPrice();
            return Json(new { totalPrice = totalPrice }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.MaSanPham == productId);
            if (item != null && quantity > 0)
            {
                item.SoLuong = quantity;  // Cập nhật số lượng
            }
            return RedirectToAction("Index");
        }
        // Checkout - Thanh toán giỏ hàng
        public ActionResult Checkout()
        {
        if (Session["userLogin"] == null)
        {
            TempData["Message"] = "Bạn cần đăng nhập để thanh toán.";
            return RedirectToAction("Login", "User");
        }

        var cart = GetCart();
        if (cart.Items.Count == 0)
        {
            return RedirectToAction("Index", "Cart");
        }

        return View(cart);
        }

        [HttpPost]
        public ActionResult Checkout(string customerName, string customerAddress, string customerPhone)
        {
            var cart = GetCart(); // Lấy giỏ hàng từ session

            

            using (QlyWebBanGiayEntities db = new QlyWebBanGiayEntities())
            {
                // Lấy mã người dùng từ session (hoặc thông tin đăng nhập)
                int MaTaiKhoan  = (int)(Session["MaNguoiDung"] ?? 0); // Giả định mã người dùng đã lưu trong session
                TaiKhoan tk = db.TaiKhoan.FirstOrDefault(mtk => mtk.MaTaiKhoan == MaTaiKhoan);
                NguoiDung nd = db.NguoiDung.FirstOrDefault(mnd => mnd.MaTaiKhoan == tk.MaTaiKhoan);
                

                // Tạo đối tượng DonHang và gán thông tin khách hàng
                DonHang order = new DonHang
                {
                    MaNguoiDung = nd.MaNguoiDung,
                    TrangThai = "Mới",
                    NgayDat = DateTime.Now,
                    TongTien = cart.GetTotalPrice(),
                };

                // Thêm đơn hàng vào cơ sở dữ liệu
                db.DonHang.Add(order);
                db.SaveChanges(); // Lưu đơn hàng trước để có mã đơn hàng
                foreach (var item in cart.Items)
                {
                    ChiTietDonHang orderDetail = new ChiTietDonHang
                    {
                        MaDonHang = order.MaDonHang,
                        MaSanPham = item.MaSanPham,
                        SoLuong = item.SoLuong,
                        DonGia = (int)item.Gia,
                        TongTien = cart.GetTotalPrice(),
                    };
                    db.ChiTietDonHang.Add(orderDetail);
                }

                db.SaveChanges();


            }

            cart.Clear(); // Xóa giỏ hàng sau thanh toán
            return RedirectToAction("OrderSuccess"); // Điều hướng đến trang xác nhận thành công
        }



        // Trang xác nhận đơn hàng thành công
        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}
