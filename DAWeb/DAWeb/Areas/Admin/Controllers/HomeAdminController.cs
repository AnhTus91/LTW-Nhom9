using DAWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAWeb.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        QlyBanGiayEntities db = new QlyBanGiayEntities();

        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            var donhang = db.DonHangs;
            int demSanPham = db.SanPhams.Count();
            int demUser = db.NguoiDungs.Count();
            int demDonHang = donhang.Count();
            


           


            ViewBag.demSanPham = demSanPham;
            ViewBag.demUser = demUser;
            ViewBag.demDonHang = demDonHang;
            
           
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["userLogin"] != null)
            {
                Session["userLogin"] = null;
                Session["hoTen"] = null;
                Session["email"] = null;
                Session["sdt"] = null;
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}