using Microsoft.AspNetCore.Mvc;
using Sunflower.Models;
using Sunflower.ViewModels;

namespace Sunflower.ViewComponents
{
    public class MenuTypeViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public MenuTypeViewComponent(Hshop2023Context context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuTypeVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            });
            return View(data); // Default.cshtml
        }
    }
}
