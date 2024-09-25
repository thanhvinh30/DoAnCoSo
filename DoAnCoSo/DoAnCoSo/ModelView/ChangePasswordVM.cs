using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView


{
    public class ChangePasswordVM
    { 
        
        [Key]
        public int CustomerId {  get; set; }
        [Display(Name = "Mật khẩu hiện tại")]
        public string PasswordNow { get; set; }
        [Display(Name ="Mật khẩu mới")]
        [Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage ="Bạn cần đặt mặt khẩu tối thiểu 5 ký tự")]
        public string PasswordNew {  get; set; }
        [Display(Name = "Nhập lại Mật khẩu mới")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mặt khẩu tối thiểu 5 ký tự")]
        [Required(ErrorMessage = "Bạn cần xác nhận lại mật khẩu mới.")]
        [Compare("PasswordNew", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ComfirmPasswordNew { get; set; }
    }
}
