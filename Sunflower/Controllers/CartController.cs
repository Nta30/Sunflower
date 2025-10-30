using Microsoft.AspNetCore.Mvc;
using Sunflower.Models;
using Sunflower.ViewModels;
using Sunflower.Helpers;

namespace Sunflower.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
        public CartController(Hshop2023Context context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int ProductId,int Quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == ProductId);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == ProductId);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Sản phẩm có mã {ProductId} không tồn tại";
                    return Redirect("/404");
                }
                else
                {
                    item = new CartItem
                    {
                        MaHh = hangHoa.MaHh,
                        TenHh = hangHoa.TenHh,
                        DonGia = hangHoa.DonGia ?? 0,
                        Hinh = hangHoa.Hinh ?? "",
                        SoLuong = Quantity
                    };
                    gioHang.Add(item);
                }
            }
            else
            {
                item.SoLuong += Quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int ProductId)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == ProductId);
            if(item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
    }
}
