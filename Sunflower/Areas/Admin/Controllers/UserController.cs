using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunflower.Helpers;
using Sunflower.Models;
using Sunflower.ViewModels;

namespace Sunflower.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly Hshop2023Context db;

        public UserController(Hshop2023Context context)
        {
            db = context;
        }
        public IActionResult Index(int? Page)
        {
            int PageSize = 15;
            int PageNumber = Page ?? 1;
            var users = db.KhachHangs.AsQueryable();

            int totalUser = users.Count();
            var result = users.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.PageNumber = PageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalUser / PageSize);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdminCreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                var check = db.KhachHangs.Find(model.MaKh);
                if(check != null)
                {
                    ModelState.AddModelError("MaKh", "Username này đã tồn tại");
                    return View(model);
                }
                var khachHang = new KhachHang
                {
                    MaKh = model.MaKh,
                    HoTen = model.HoTen,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    HieuLuc = model.HieuLuc,
                    VaiTro = model.VaiTro
                };
                khachHang.RandomKey = MyUtil.GenerateRandomKey();
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                if(model.Hinh != null)
                {
                    khachHang.Hinh = MyUtil.UpLoadImage(model.Hinh, "KhachHang");
                }
                db.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(string UserId)
        {
            var user = db.KhachHangs.Find(UserId);
            if(user == null)
            {
                TempData["Message"] = "User not found";
                return Redirect("/404");
            }
            db.KhachHangs.Remove(user);
            db.SaveChanges();
            TempData["Message"] = "Delete succesfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string UserName)
        {
            if(UserName == null)
            {
                TempData["Message"] = "User not found";
                return Redirect("/404");
            }
            var khachHang = db.KhachHangs.Find(UserName);
            if(khachHang == null)
            {
                TempData["Message"] = "User not found";
                return Redirect("/404");
            }
            var model = new AdminManageRolesVM
            {
                MaKh = khachHang.MaKh,
                HoTen = khachHang.HoTen,
                Email = khachHang.Email,
                VaiTro = khachHang.VaiTro,
                HieuLuc = khachHang.HieuLuc
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(AdminManageRolesVM model)
        {
            var khachHang = db.KhachHangs.Find(model.MaKh);
            if(khachHang == null)
            {
                TempData["Message"] = "User not found";
                return Redirect("/404");
            }
            khachHang.VaiTro = model.VaiTro;
            khachHang.HieuLuc = model.HieuLuc;
            db.KhachHangs.Update(khachHang);
            db.SaveChanges();
            TempData["Message"] = "Delete succesfully";
            return RedirectToAction("Index");
        }
    }
}
