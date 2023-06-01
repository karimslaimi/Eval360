using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class CompagnieUser
    {
        [Key]
        public int id { get; set; }

        public int idCompagnie { get; set; }
        [ForeignKey("idCompagnie")]
        public Compagnie compagnie { get; set; }

        public string? userId { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; } 

    }
}
