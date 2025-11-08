using System.ComponentModel.DataAnnotations;

namespace Sunflower.ViewModels
{
    public class InvoiceEditVM
    {
        public int MaHd { get; set; }

        [Display(Name = "Order Status")]
        public int MaTrangThai { get; set; }

        [Display(Name = "Admin Notes")]
        public string? GhiChu { get; set; }

        public string HoTenNguoiDat { get; set; }
        public DateTime NgayDat { get; set; }
    }
}
