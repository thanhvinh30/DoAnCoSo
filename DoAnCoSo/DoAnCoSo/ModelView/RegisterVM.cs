using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System;


namespace DoAnCoSo.ModelView
{
    public class RegisterVM
    {
        public int CustomerId { get; set; }
        [Display(Name ="Họ và tên")]
        [Required(ErrorMessage ="Vui lòng nhập họ tên")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập Số điện thoại hoặc Email")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail: ", controller: "Account")]
        public string Email { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        [Display(Name ="Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action:"validatePhone", controller:"Account")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required( ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength( 10, ErrorMessage ="Bạn cần đặt số điện thoại tối thiểu 10 ký tự")]
        public string Password { get; set; }
        [MinLength(15, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 10 ký tự")]
        [Display(Name ="Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage ="Vui lòng nhập mật khẩu giống nhau ")]
        public string ConfirmPassword { get; set; }
        public string PasswordHash { get; set; }
        public bool? Active { get; set; }
        [Display(Name ="Ngày tạo")]
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
