using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class Poste
    {
        [Key] 
        public int Id { get; set; }

        public string libelle { get; set; }

        public int niveau { get; set; }

        [ForeignKey("Direction")]
        public int IdDirection { get; set; }

        public Direction Direction { get; set; }
    }
}
