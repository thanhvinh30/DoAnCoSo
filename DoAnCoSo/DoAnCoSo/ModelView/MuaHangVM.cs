using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView
{
    public class MuaHangVM
    {
        [Key]   
        public int CustomerId { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập họ và tên")]
        public string FullName { get; set; }
        public string Email {  get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Tỉnh / Thành")]
        public int TinhThanh { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn Quận / Huyện")]
        public int QuanHuyen { get; set; }
        [Required(ErrorMessage = "Vui lòng Phường / Xã")]
        public int PhuongXa { get; set; }
        public int PaymentID { get; set; }
        public string Note { get; set; }

    }
}
