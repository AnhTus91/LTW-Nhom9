using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAWeb.Models;

namespace DAWeb.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private QlyBanGiayEntities db = new QlyBanGiayEntities();

        

        // GET: Admin/SanPhams
        public ActionResult Index()
        {
            var sanPham = db.SanPhams.Include(s => s.DanhMuc);
            return View(sanPham.ToList());
        }

        // GET: Admin/SanPhams/Details/5
        public ActionResult Details(int id)
        {
            // Tìm sản phẩm theo ID
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy sản phẩm, trả về lỗi 404
            }
            return View(sanPham); // Truyền dữ liệu sản phẩm vào View chi tiết
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase AnhSanPham)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra file ảnh được upload
                if (AnhSanPham != null && AnhSanPham.ContentLength > 0)
                {
                    // Lấy tên file và định danh duy nhất cho ảnh
                    var idImage = Guid.NewGuid().ToString();
                    var fileName = idImage + Path.GetExtension(AnhSanPham.FileName);

                    // Đường dẫn tới thư mục 'images' trong dự án
                    var folderPath = Server.MapPath("~/images");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath); // Tạo thư mục nếu chưa có
                    }

                    var path = Path.Combine(folderPath, fileName);
                    AnhSanPham.SaveAs(path); // Lưu ảnh vào thư mục 'images'

                    // Lưu đường dẫn ảnh vào cơ sở dữ liệu (đường dẫn tương đối)
                    sanPham.AnhSanPham = "/images/" + fileName;
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }



        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase AnhSanPham)
        {
            if (ModelState.IsValid)
            {
                if (AnhSanPham != null && AnhSanPham.ContentLength > 0)
                {
                    var idImage = Guid.NewGuid().ToString();
                    var fileName = idImage + Path.GetExtension(AnhSanPham.FileName);
                    var folderPath = Server.MapPath("~/images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var path = Path.Combine(folderPath, fileName);
                    AnhSanPham.SaveAs(path);

                    // Cập nhật đường dẫn ảnh mới
                    sanPham.AnhSanPham = "/images/" + fileName;
                }

                db.Entry(sanPham).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDanhMuc = new SelectList(db.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
