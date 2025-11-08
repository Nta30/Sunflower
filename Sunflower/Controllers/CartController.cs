using Microsoft.AspNetCore.Mvc;
using Sunflower.Models;
using Sunflower.ViewModels;
using Sunflower.Helpers;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        public IActionResult Index()
        {
            return View(Cart);
        }

        [Authorize]
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

        [Authorize]

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

        [Authorize]
        public IActionResult UpdateQuantity(int ProductId, int Quantity)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == ProductId);
            if(item != null)
            {
                if(Quantity <= 0)
                {
                    gioHang.Remove(item);
                }
                else
                {
                    item.SoLuong = Quantity;
                }
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return Json(new { success = true });
        }
    }
}
