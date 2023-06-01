using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class Poste
    {
        [Key] 
        public int Id { get; set; }

        public string libelle { get; set; }


        
        public int? IdDirection { get; set; }
        [ForeignKey("IdDirection")]
        public Direction? Direction { get; set; }

        public ICollection<User>? users { get; set; }
    }
}
