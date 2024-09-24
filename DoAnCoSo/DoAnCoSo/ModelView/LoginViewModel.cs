using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView
{
    public class LoginViewModel
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string UserName  { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage ="Bạn nhập mật khẩu tối thiểu là 5 ký tự")]
        public string Password { get; set; }
    }
}
