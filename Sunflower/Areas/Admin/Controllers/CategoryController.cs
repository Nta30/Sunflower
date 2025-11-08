using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Sunflower.Helpers;
using Sunflower.Models;
using Sunflower.ViewModels;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sunflower.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly Hshop2023Context db;
        public CategoryController(Hshop2023Context context)
        {
            db = context;
        }
        public IActionResult Index(int? Page)
        {
            int PageSize = 15;
            int PageNumber = Page ?? 1;

            var hangHoas = db.HangHoas.AsQueryable();

            int totalProducts = hangHoas.Count();

            var result = hangHoas.Skip((PageNumber - 1) * PageSize)
                                .Take(PageSize)
                                .Select(p => new AdminCategoryVM
                                {
                                    MaHh = p.MaHh,
                                    TenHh = p.TenHh,
                                    DonGia = p.DonGia ?? 0,
                                    NgaySx = p.NgaySx.ToString("dd/MM/yyyy") ?? "",
                                    GiamGia = p.GiamGia,
                                    SoLanXem = p.SoLanXem
                                }).ToList();

            ViewBag.PageNumber = PageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / PageSize);

            return View(result);
        }

        [HttpGet]
        public IActionResult EditProduct(int ProductId)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            var data = db.HangHoas.SingleOrDefault(p => p.MaHh == ProductId);
            var sanPham = new AdminProductVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                TenAlias = data.TenAlias,
                MaLoai = data.MaLoai,
                MoTaDonVi = data.MoTaDonVi,
                DonGia = data.DonGia,
                Hinh = data.Hinh,
                NgaySx = data.NgaySx,
                GiamGia = data.GiamGia,
                SoLanXem = data.SoLanXem,
                MoTa = data.MoTa,
                MaNcc = data.MaNcc
            };
            return View(sanPham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(AdminProductVM product)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            try
            {
                if (ModelState.IsValid)
                {
                    var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == product.MaHh);
                    hangHoa.MaHh = product.MaHh;
                    hangHoa.TenHh = product.TenHh;
                    hangHoa.TenAlias = product.TenAlias;
                    hangHoa.MaLoai = product.MaLoai;
                    hangHoa.MoTaDonVi = product.MoTaDonVi;
                    hangHoa.DonGia = product.DonGia;
                    hangHoa.NgaySx = product.NgaySx;
                    hangHoa.GiamGia = product.GiamGia;
                    hangHoa.SoLanXem = product.SoLanXem;
                    hangHoa.MoTa = product.MoTa;
                    hangHoa.MaNcc = product.MaNcc;

                    if(product.HinhUpload != null)
                    {
                        hangHoa.Hinh = MyUtil.UpLoadImage(product.HinhUpload, "HangHoa");
                    }

                    db.Entry(hangHoa).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(product);
        }

        public IActionResult AddProduct()
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(AdminProductVM product)
        {
            ViewBag.MaLoai = new SelectList(db.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.MaNcc = new SelectList(db.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");

            if(product.HinhUpload == null)
            {
                ModelState.AddModelError("HinhUpLoad", "Product Image is required");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var hangHoa = new HangHoa
                    {
                        MaHh = product.MaHh,
                        TenHh = product.TenHh,
                        TenAlias = product.TenAlias,
                        MaLoai = product.MaLoai,
                        MoTaDonVi = product.MoTaDonVi,
                        DonGia = product.DonGia,
                        NgaySx = product.NgaySx,
                        GiamGia = product.GiamGia,
                        MoTa = product.MoTa,
                        MaNcc = product.MaNcc
                    };
                    product.SoLanXem = 0;
                    if (product.HinhUpload != null)
                    {
                        hangHoa.Hinh = MyUtil.UpLoadImage(product.HinhUpload, "HangHoa");
                    }
                    db.HangHoas.Add(hangHoa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(product);
        }

        public IActionResult DeleteProduct(int ProductId)
        {
            var chiTietHoaDon = db.ChiTietHds.Where(p => p.MaHh == ProductId).ToList();
            if(chiTietHoaDon.Count > 0)
            {
                TempData["Message"] = "Can't delete item";
                return RedirectToAction("Index", "Category");
            }
            db.Remove(db.HangHoas.Find(ProductId));
            db.SaveChanges();
            TempData["Message"] = "Delete succesfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
