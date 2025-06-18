using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuesthouseWeb.Models
{
    [Table("Inquiry")]
    public class Inquiry
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Column("email")]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [Column("mobile")]
        [StringLength(50)]
        public string Mobile { get; set; }

        [Required]
        [Column("content")]
        public string Content { get; set; }

        [Required]
        [Column("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("ans_yn")]
        [StringLength(1)]
        public string AnsYn { get; set; } = "N";
    }
}
