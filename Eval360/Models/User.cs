using Microsoft.AspNetCore.Identity;

namespace Eval360.Models
{
    public class User : IdentityUser
    {
        public string? cin { get;set; }
        public string? Nom { get; set; }
        public string? preNom { get; set; }
        public string? sexe { get; set; }
        public UserType UserType { get; set; }
        public DateTime? dateEmbauche { get; set; }

        public int? idPoste { get; set; }
        public Poste? Poste { get; set; }

        public string? idSuperior { get; set; }
        public User? superior { get; set; }
        public List<Compagnie> compagnies { get; set; }
        public List<CompagnieReponse> compagnieReponses { get; set; }
        public List<CompagnieUser> compagnieUser { get; set; }

      public User() { }
    }
}
