using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sunflower.Models;
using Sunflower.ViewModels;
using System;
using System.Linq;

namespace Sunflower.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InvoiceController : Controller
    {
        private readonly Hshop2023Context db;

        public InvoiceController(Hshop2023Context context)
        {
            db = context;
        }

        public IActionResult Index(int? Page)
        {
            int PageSize = 15;
            int PageNumber = Page ?? 1;

            var hoaDonsQuery = db.HoaDons
                                 .Include(hd => hd.MaKhNavigation)
                                 .Include(hd => hd.MaTrangThaiNavigation)
                                 .OrderByDescending(hd => hd.NgayDat)
                                 .AsQueryable();

            int totalInvoices = hoaDonsQuery.Count();
            var result = hoaDonsQuery.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.PageNumber = PageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalInvoices / PageSize);

            return View(result);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hoaDon = db.HoaDons.Include(hd => hd.MaKhNavigation)
                                   .FirstOrDefault(hd => hd.MaHd == id);

            if (hoaDon == null)
            {
                TempData["Message"] = "Invoice not found";
                return Redirect("/404");
            }

            var model = new InvoiceEditVM
            {
                MaHd = hoaDon.MaHd,
                MaTrangThai = hoaDon.MaTrangThai,
                GhiChu = hoaDon.GhiChu,
                HoTenNguoiDat = hoaDon.HoTen ?? hoaDon.MaKhNavigation.HoTen,
                NgayDat = hoaDon.NgayDat
            };

            ViewBag.TrangThaiList = new SelectList(
                db.TrangThais.ToList(),
                "MaTrangThai",
                "TenTrangThai",
                hoaDon.MaTrangThai
            );

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(InvoiceEditVM model)
        {
            var hoaDon = db.HoaDons.Find(model.MaHd);
            if (hoaDon == null)
            {
                TempData["Message"] = "Invoice not found";
                return Redirect("/404");
            }

            hoaDon.MaTrangThai = model.MaTrangThai;
            hoaDon.GhiChu = model.GhiChu;

            if (model.MaTrangThai == 4 && hoaDon.NgayGiao == null)
            {
                hoaDon.NgayGiao = DateTime.Now;
            }

            db.HoaDons.Update(hoaDon);
            db.SaveChanges();
            TempData["Message"] = "Updated succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var hoaDon = db.HoaDons.Find(id);
            if (hoaDon == null)
            {
                TempData["Message"] = "Invoice not found";
                return Redirect("/404");
            }

            try
            {
                var chiTietHds = db.ChiTietHds.Where(ct => ct.MaHd == id).ToList();
                if (chiTietHds.Any())
                {
                    db.ChiTietHds.RemoveRange(chiTietHds);
                }

                db.HoaDons.Remove(hoaDon);
                db.SaveChanges();
                TempData["Message"] = "Delete succesfully";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Can't delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var hoaDon = db.HoaDons
                .Include(hd => hd.MaKhNavigation)
                .Include(hd => hd.MaTrangThaiNavigation)
                .FirstOrDefault(m => m.MaHd == id);

            if (hoaDon == null) return Redirect("/404");

            var chiTiet = db.VChiTietHoaDons
                .Where(ct => ct.MaHd == id)
                .ToList();

            var model = new InvoiceDetailVM
            {
                HoaDon = hoaDon,
                ChiTietHds = chiTiet
            };

            return View(model);
        }
    }
}
