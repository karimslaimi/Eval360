namespace Eval360.Models
{
    public class Compagnie
    {
        public int id { get; set; }
        public DateTime dateDebut { get; set; }
        public DateTime dateFin { get; set; }
        public User employee { get; set; }
        public List<CompagnieQuestion> compagnieQuestions { get; set; } = new();
    }
}
