using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System;


namespace DoAnCoSo.ModelView
{
    public class RegisterVM
    {
        public int CustomerId { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FulllName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail: ", controller: "Account")]
        public string Email { get; set; }
        [MaxLength(10)]
        [Required(ErrorMessage = "Vui lòng nhập Số Điện Thoại")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "validatePhone", controller: "Account")]
        public string Phone { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(10, ErrorMessage = "Bạn cần đặt số điện thoại tối thiểu 10 ký tự")]
        public string Password { get; set; }
        [MinLength(15, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 10 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau ")]
        public string ConfirmPassword { get; set; }
    }
}
