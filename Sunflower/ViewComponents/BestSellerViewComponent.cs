using Microsoft.AspNetCore.Mvc;
using Sunflower.Models;
using Sunflower.ViewModels;

namespace Sunflower.ViewComponents
{
    public class BestSellerViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public BestSellerViewComponent(Hshop2023Context context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.HangHoas
                .OrderByDescending(p => p.SoLanXem)
                .Take(9)
                .Select(p => new ProductVM
                {
                    Mahh = p.MaHh,
                    Tenhh = p.TenHh,
                    Dongia = p.DonGia ?? 0,
                    Hinh = p.Hinh ?? "",
                    MotaNgan = p.MoTaDonVi ?? "",
                    TenLoai = p.MaLoaiNavigation.TenLoai
                });

            return View(data);
        }
    }
}
