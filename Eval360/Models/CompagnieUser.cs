using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class CompagnieUser
    {
        [Key]
        public int id { get; set; }
        public Compagnie compagnie { get; set; }
        public User user { get; set; } 

    }
}
