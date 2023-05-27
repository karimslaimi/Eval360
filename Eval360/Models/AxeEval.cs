using System.ComponentModel.DataAnnotations;

namespace Eval360.Models
{
    public class AxeEval
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Question>? questions { get; set; }
    }
}
