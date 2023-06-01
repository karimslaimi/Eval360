using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class CompagnieQuestion
    {
        [Key]
        public int id { get; set; }

        public int compagnieId { get; set; }
        [ForeignKey("compagnieId")]
        public Compagnie compagnie { get; set; }


        public int questionId { get; set; }
        [ForeignKey("questionId")]
        public Question question { get; set; }


        public List<CompagnieReponse> reponses { get; set; }
    }
}
