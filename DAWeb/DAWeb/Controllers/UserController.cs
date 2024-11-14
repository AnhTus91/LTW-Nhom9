using DAWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DAWeb.Controllers
{
    public class UserController : Controller
    {
        QlyWebBanGiayEntities db = new QlyWebBanGiayEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: Đăng ký
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email đã tồn tại chưa
                var existingAccount = db.TaiKhoan.FirstOrDefault(s => s.Email == model.Email);
                if (existingAccount == null)
                {
                    // Tạo tài khoản mới
                    TaiKhoan newAccount = new TaiKhoan
                    {
                        TenDangNhap = model.TenDangNhap,
                        Email = model.Email,
                        MatKhau = model.MatKhau, // Có thể mã hóa mật khẩu trước khi lưu vào DB
                        VaiTro = "user" // Gán vai trò là người dùng thông thường
                    };

                    db.TaiKhoan.Add(newAccount);
                    db.SaveChanges();

                    // Tạo thông tin người dùng tương ứng với tài khoản
                    

                    return RedirectToAction("Login");
                }
                else
                {
                    // Email đã tồn tại
                    ViewBag.error = "Email đã tồn tại";
                }
            }

            return View(model);
        }


        // GET: Đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                var account = db.TaiKhoan
                    .FirstOrDefault(s => s.TenDangNhap == model.TenDangNhap && s.MatKhau == model.MatKhau);

                if (account != null)
                {
                    // Lưu thông tin vào Session
                    Session["userLogin"] = account.Email;
                    Session["MaNguoiDung"] = account.MaTaiKhoan;
                    Session["VaiTro"] = account.VaiTro;

                    // Điều hướng dựa trên vai trò
                    if (account.VaiTro == "admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                    else if (account.VaiTro == "user")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            return View(model);
        }



        // GET: Đăng xuất
        public ActionResult Logout()
        {
            if (Session["userLogin"] != null)
            {
                Session["userLogin"] = null;
                Session["hoTen"] = null;
                Session["email"] = null;
                Session["sdt"] = null;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Chi tiết người dùng
        public ActionResult UserDetails()
        {
            string email = Session["userLogin"]?.ToString();
            if (email == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Lấy tài khoản và thông tin người dùng
            TaiKhoan taiKhoan = db.TaiKhoan.FirstOrDefault(tk => tk.Email == email);
            NguoiDung nguoiDung = db.NguoiDung.FirstOrDefault(nd => nd.MaTaiKhoan == taiKhoan.MaTaiKhoan);

            // Nếu người dùng chưa có thông tin thì chuyển sang CreateUserDetails
            if (nguoiDung == null)
            {
                return RedirectToAction("CreateUserDetails", "User");
            }

            return View(nguoiDung);
        }

        // GET: Tạo thông tin người dùng
        public ActionResult CreateUserDetails()
        {
            string email = Session["userLogin"]?.ToString();


            // Kiểm tra tài khoản
            TaiKhoan taiKhoan = db.TaiKhoan.FirstOrDefault(tk => tk.Email == email);
            if (taiKhoan == null)
            {
                return RedirectToAction("Login", "User");
            }
            // Tạo một đối tượng người dùng mới
            NguoiDung nguoiDung = new NguoiDung
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
            };

            return View(nguoiDung);
        }

        // POST: Tạo thông tin người dùng
        [HttpPost]
        public ActionResult CreateUserDetails(NguoiDung nguoiDung, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string directoryPath = Server.MapPath("~/Images");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    
                    string path = Path.Combine(directoryPath, fileName);
                    ImageFile.SaveAs(path);
                    nguoiDung.AnhDaiDien = "/Images/" + fileName; // Save the path to the image in the model

                }
                // Lưu thông tin người dùng vào cơ sở dữ liệu
                db.NguoiDung.Add(nguoiDung);
                db.SaveChanges();

                // Sau khi lưu, chuyển hướng đến trang chi tiết người dùng
                return RedirectToAction("UserDetails","User");
            }

            return View(nguoiDung);
        }

        // GET: Chỉnh sửa thông tin người dùng
        public ActionResult EditUserDetails()
        {
            string email = Session["userLogin"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "User");
            }

            // Lấy tài khoản
            TaiKhoan taiKhoan = db.TaiKhoan.FirstOrDefault(tk => tk.Email == email);
            NguoiDung nguoiDung = db.NguoiDung.FirstOrDefault(nd => nd.MaTaiKhoan == taiKhoan.MaTaiKhoan);


            return View(nguoiDung);
        }

        // POST: Chỉnh sửa thông tin người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserDetails(NguoiDung nguoiDung, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Đường dẫn thư mục lưu trữ ảnh
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);

                    // Lưu file vào thư mục
                    ImageFile.SaveAs(path);

                    // Cập nhật đường dẫn ảnh vào model
                    nguoiDung.AnhDaiDien = Path.Combine("/Images", fileName);
                }
                // Tìm người dùng hiện tại trong cơ sở dữ liệu
                var existingUser = db.NguoiDung.Find(nguoiDung.MaNguoiDung);
                if (existingUser != null)
                {
                    existingUser.HoTen = nguoiDung.HoTen;
                    existingUser.DiaChi = nguoiDung.DiaChi;
                    existingUser.SoDienThoai = nguoiDung.SoDienThoai;

                    db.Entry(existingUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("UserDetails", new { id = nguoiDung.MaNguoiDung });
            }

            return View(nguoiDung);
        }
        //đơn hàng 
        public ActionResult OrderHistory(int id)
        {
            if (Session["MaNguoiDung"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            var donHangList = db.DonHang.Where(dh => dh.MaNguoiDung == id).OrderByDescending(dh => dh.NgayDat).ToList();

            return View(donHangList);
        }
        public ActionResult OrderDetails(int id)
        {
            var donHang = db.DonHang.Include("ChiTietDonHang.SanPham").FirstOrDefault(d => d.MaDonHang == id);


            if (donHang == null)
            {
                return HttpNotFound();
            }

            ViewBag.ChiTietDonHang = donHang.ChiTietDonHang.ToList();
            return View(donHang);
        }

        public ActionResult CancelOrder(int id)
        {
            
            var donHang = db.DonHang.FirstOrDefault(dh => dh.MaDonHang == id);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            if (donHang.TrangThai == "Mới")
            {
                donHang.TrangThai = "Đã hủy";
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Đơn hàng không thể hủy.";
            }

            return RedirectToAction("OrderHistory", new { id = donHang.MaNguoiDung });
        }
    }


}
