using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class Direction
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Poste>? postes { get; set; }
    }
}
