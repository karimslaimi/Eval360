using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class Question : IEquatable<Question>
    {
        [Key]
        public int id { get; set; }
        public string libelle { get; set; }
        public bool isEnabled { get; set; }

        public int idEval { get; set; }
        [ForeignKey("idEval")]
        public AxeEval axeEval { get; set; }

        public List<CompagnieQuestion> compagnieQuestions { get; set; } = new();

        public bool Equals(Question? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return other.id == this.id;
        }
    }
}
