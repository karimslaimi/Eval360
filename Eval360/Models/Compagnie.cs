using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class Compagnie
    {
        public int id { get; set; }
        public string title { get; set; }
        public string qualiteEvaluateur { get; set; }
        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }
        public string? userId { get; set; }
        [ForeignKey("userId")]
        public User employee { get; set; }
        public List<CompagnieQuestion> compagnieQuestions { get; set; } 
        public List<CompagnieUser> compagnieUser { get; set; } 
    }
}
