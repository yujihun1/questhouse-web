using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuesthouseWeb.Models
{
    public class User
    {
        [Required]
        [Key]
        public string User_id { get; set; }

        [Required]
        public string User_nm { get; set; }

        [Required]
        public string User_ip { get; set; }
    }
}
