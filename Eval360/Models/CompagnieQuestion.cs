using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class CompagnieQuestion
    {
        [Key]
        public int id { get; set; }
        public Compagnie compagnie { get; set; }
        public Question question { get; set; }
        public List<CompagnieReponse> reponses { get; set; }
    }
}
