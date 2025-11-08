namespace Sunflower.ViewModels
{
    public class EditProfileVM
    {
        public string MaKh { get; set; }
        public string HoTen { get; set; }
        public bool GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }

        public string? HinhHienTai { get; set; }
        public IFormFile? HinhMoi { get; set; }
    }
}
