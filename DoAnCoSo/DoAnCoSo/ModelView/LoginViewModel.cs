using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView
{
    public class LoginViewModel
    {
        [MaxLength(100)]
        [Required(ErrorMessage ="Vui lòng nhập Số điện thoại hoặc Email")]
        [Display(Name ="Điện thoại / Email")]
        public string UserName  { get; set; }

        [Display(Name ="Mật Khẩu: ")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu ")]
        [MinLength(15, ErrorMessage ="Bạn nhập mật khẩu tối thiểu là 15 ký tự")]
        public string Password { get; set; }
    }
}
