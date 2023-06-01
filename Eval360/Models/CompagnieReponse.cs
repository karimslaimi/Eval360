using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eval360.Models
{
    public class CompagnieReponse
    {
        [Key]
        public int id { get; set; }

        public int compagnieQuestionId { get; set; }

        [ForeignKey("compagnieQuestionId")]
        public CompagnieQuestion CompagnieQuestion { get; set; }
        public string? userId { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }
        public int note { get; set; }
    }
}
