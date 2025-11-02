using System.Text;

namespace Sunflower.Helpers
{
    public class MyUtil
    {
        public static string UpLoadImage(IFormFile Hinh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Hinh", folder, Hinh.FileName);
                using (var myFile = new FileStream(fullPath, FileMode.Create))
                {
                    Hinh.CopyTo(myFile);
                }
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GenerateRandomKey(int length = 5)
        {
            var parttern = @"qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i=0; i< length; i++)
            {
                sb.Append(parttern[rd.Next(0, parttern.Length)]);
            }
            return sb.ToString();
        }
    }
}
