using System.ComponentModel.DataAnnotations;

namespace WebApiNet5CodeFirst.Models
{
    public class LoaiModel
    {
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
