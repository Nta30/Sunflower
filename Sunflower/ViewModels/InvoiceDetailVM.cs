using Sunflower.Models;
using System.Collections.Generic;

namespace Sunflower.ViewModels
{
    public class InvoiceDetailVM
    {
        public HoaDon HoaDon { get; set; }
        public IEnumerable<VChiTietHoaDon> ChiTietHds { get; set; }
    }
}