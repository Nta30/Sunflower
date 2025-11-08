using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Sunflower.ViewModels
{
    public class AdminProductVM
    {
        [Required]
        public int MaHh { get; set; }
        [Required]
        public string TenHh { get; set; } = null!;

        public string? TenAlias { get; set; }
        [Required]

        public int MaLoai { get; set; }

        public string? MoTaDonVi { get; set; }

        public double? DonGia { get; set; }

        public string? Hinh { get; set; }

        public IFormFile? HinhUpload { get; set; }

        public DateTime NgaySx { get; set; }

        public double GiamGia { get; set; }

        public int SoLanXem { get; set; }

        public string? MoTa { get; set; }
        [Required]

        public string MaNcc { get; set; }
    }
}
