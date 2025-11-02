using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sunflower.Helpers;
using Sunflower.Models;
using Sunflower.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sunflower.Controllers
{
    public class AccountController : Controller
    {
        private readonly Hshop2023Context db;
        public AccountController(Hshop2023Context context)
        {
            db = context;
        }

        #region
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(RegisterVM model, IFormFile Hinh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var khachHang = new KhachHang
                    {
                        MaKh = model.MaKh,
                        HoTen = model.HoTen,
                        GioiTinh = model.GioiTinh,
                        NgaySinh = (DateTime)model.NgaySinh,
                        DiaChi = model.DiaChi,
                        DienThoai = model.DienThoai,
                        Email = model.Email
                    };
                    khachHang.RandomKey = MyUtil.GenerateRandomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;

                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UpLoadImage(Hinh, "KhachHang");
                    }
                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        #endregion

        #region
        [HttpGet]
        public IActionResult SignIn(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        public async Task<IActionResult> SignIn(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.Username);
                if(khachHang == null)
                {
                    ModelState.AddModelError("Error", "Username does not exist");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Error", "our account has been locked. Please contact the administrator for assistance");
                    }
                    else
                    {
                        if(khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("Error", "Invalid username or password");
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim("CustomerID", khachHang.MaKh),
                                new Claim(ClaimTypes.Gender, khachHang.GioiTinh==true ?"Male":"Female"),
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.DateOfBirth, khachHang.NgaySinh.ToString("yyyy-MM-dd")),
                                new Claim(ClaimTypes.StreetAddress, khachHang.DiaChi ?? ""),
                                new Claim(ClaimTypes.MobilePhone, khachHang.DienThoai ?? ""),
                                new Claim("ProfileImage", khachHang.Hinh ?? ""),
                                new Claim(ClaimTypes.Role, khachHang.VaiTro == 1 ? "Admin" : "User")
                            };
                            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

                            await HttpContext.SignInAsync(claimPrincipal);

                            if (khachHang.VaiTro==1)
                            {
                                return RedirectToAction("Index", "Home", new {area = "Admin"});
                            }

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var model = new ProfileVM()
            {
                HoTen = User.FindFirstValue(ClaimTypes.Name) ?? "",
                MaKh = User.FindFirstValue("CustomerID") ?? "",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                NgaySinh = User.FindFirstValue(ClaimTypes.DateOfBirth) ?? "",
                GioiTinh = User.FindFirstValue(ClaimTypes.Gender) ?? "",
                DiaChi = User.FindFirstValue(ClaimTypes.StreetAddress) ?? "",
                DienThoai = User.FindFirstValue(ClaimTypes.MobilePhone) ?? "",
                Hinh = User.FindFirstValue("ProfileImage") ?? "",
                VaiTro = User.FindFirstValue(ClaimTypes.Role) ?? ""
            };

            return View(model);
        }
    }
}
