using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class CompagnieReponse
    {
        [Key]
        public int id { get; set; }
        public CompagnieQuestion CompagnieQuestion { get; set; }
        public User user { get; set; }
        public int note { get; set; }
    }
}
