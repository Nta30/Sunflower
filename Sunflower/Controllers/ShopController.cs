using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sunflower.Models;
using Sunflower.ViewModels;
using System.Net.WebSockets;
using System.Security.Cryptography.Pkcs;

namespace Sunflower.Controllers
{
    public class ShopController : Controller
    {
        private readonly Hshop2023Context db;
        public ShopController(Hshop2023Context context) {
            db = context;
        }
        public IActionResult Index(int? ProductType)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (ProductType.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == ProductType.Value);
            }
            var result = hangHoas.Select(p => new ProductVM
            {
                Mahh = p.MaHh,
                Tenhh = p.TenHh,
                Dongia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MotaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new ProductVM
            {
                Mahh = p.MaHh,
                Tenhh = p.TenHh,
                Dongia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MotaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View("Index",result);
        }

        public IActionResult Detail(int ProductId)
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == ProductId);
            if (data == null)
            {
                TempData["Message"] = $"Sản phẩm có mã {ProductId} không tồn tại";
                return Redirect("/404");
            }
            var result = new ProductDetailVM
            {
                Mahh = data.MaHh,
                Tenhh = data.TenHh,
                Dongia = data.DonGia ?? 0,
                Hinh = data.Hinh ?? "",
                MotaNgan = data.MoTaDonVi ?? "",
                ChiTiet = data.MoTa ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                DiemDanhGia = 5
            };
            return View(result);
        }
    }
}
