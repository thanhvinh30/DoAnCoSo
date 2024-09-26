using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace DoAnCoSo.ModelView
{
    public class MuaHangVM
    {
        [Key]   
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
