namespace Sunflower.ViewModels
{
    public class ProductVM
    {
        public int Mahh { get; set; }
        public string Tenhh { get; set; }
        public string Hinh { get; set; }
        public double Dongia { get; set; }
        public string MotaNgan { get; set; }
        public string TenLoai { get; set; }
    }

    public class ProductDetailVM
    {
        public int Mahh { get; set; }
        public string Tenhh { get; set; }
        public string Hinh { get; set; }
        public double Dongia { get; set; }
        public string MotaNgan { get; set; }
        public string ChiTiet { get; set; }
        public string TenLoai { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
    }
}
