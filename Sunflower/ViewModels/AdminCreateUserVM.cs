using System.ComponentModel.DataAnnotations;

namespace Sunflower.ViewModels
{
    public class AdminCreateUserVM
    {
        [Display(Name = "Username")]
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters")]
        public string MaKh { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters")]
        public string HoTen { get; set; }

        [Display(Name = "Gender")]
        public bool GioiTinh { get; set; } = true;

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date Of Birth is required")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60, ErrorMessage = "Maximum 60 characters")]
        public string? DiaChi { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(24, ErrorMessage = "Maximum 24 characters")]
        [RegularExpression("^0(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$",
            ErrorMessage = "Invalid phone number format")]
        public string? DienThoai { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? Hinh { get; set; }

        [Display(Name = "Active")]
        public bool HieuLuc { get; set; } = true;

        [Display(Name = "Role")]
        public int VaiTro { get; set; } = 0;
    }
}
