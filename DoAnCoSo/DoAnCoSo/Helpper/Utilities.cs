using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoAnCoSo.Helpper
{
    public static class Utilities
    {

        public static int PAGE_SIZE = 6;
        public static void CreateIfMissing(string path)                     // đây là lệnh tạo mới cho những cái chưa có ( trích từ lời video ) :>>
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        //private static void CreateIfMissing(string path)
        //{
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //}
        public static string ToTitleCase(string str)
        {
            String result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
            }
            return result;
        }
        public static string GetRanDomKey(int length = 5)
        {
            string pattern = "";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            url = Regex.Replace(url, @"[éèẻẽẹêếềểễệ]", "e");
            url = Regex.Replace(url, @"[óòỏõọôốồổỗộơớờởỡợ]", "o");
            url = Regex.Replace(url, @"[íìỉĩị]", "i");
            url = Regex.Replace(url, @"[ýỳỷỹỵ]", "y");
            url = Regex.Replace(url, @"[úùủũụưứừửữự]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            // 2. Chỉ cho phép nhận [0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();

            // Xử lý nhiều hơn 1 khoảng trắng --> 1 ký tự
            url = Regex.Replace(url.Trim(), @"\s+", "-");

            // Thay khoảng trắng bằng dấu gạch ngang
            url = Regex.Replace(url, @"\s", "-");

            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }

            return url;
        }
        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname = null)
        {
            try
            {
                // Kiểm tra file null hoặc rỗng
                if (file == null || file.Length == 0)
                {
                    return null; // Không có file upload hoặc file rỗng
                }

                // Set default name if newname is not provided
                if (newname == null)
                    newname = file.FileName;

                // Create the directory path and ensure it exists
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img-PhuTungXe(BanMoi)", sDirectory);
                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                // Define full file path including the new name
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img-PhuTungXe(BanMoi)", sDirectory, newname);

                // Define supported file types
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1);


                // Check if file extension is supported
                if (!supportedTypes.Contains(fileExt.ToLower()))
                {
                    return null; // Unsupported file type
                }

                // Kiểm tra định dạng file
                if (string.IsNullOrEmpty(fileExt) || !supportedTypes.Contains(fileExt.ToLower()))
                {
                    return null; // Nếu phần mở rộng không hợp lệ
                }
                else
                {
                    // Save the file asynchronously
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname; // Return the file path if upload is successful
                }


            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết
                Console.WriteLine(ex.Message);
                // Handle exceptions (optional: log error)
                return null;
            }
        }
    }
}
