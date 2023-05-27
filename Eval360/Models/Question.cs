using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class Question
    {
        [Key]
        public int id { get; set; }
        public string libelle { get; set; }
        public bool isEnabled { get; set; }
        public AxeEval axeEval { get; set; }

        public List<CompagnieQuestion> compagnieQuestions { get; set; } = new();
    }
}
