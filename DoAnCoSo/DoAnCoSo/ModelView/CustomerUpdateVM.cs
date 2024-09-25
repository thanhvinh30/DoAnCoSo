using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView
{
    public class CustomerUpdateVM
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Họ và Tên không được để trống")]
        public string CusName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
    }
}
