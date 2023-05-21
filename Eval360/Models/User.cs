using Microsoft.AspNetCore.Identity;

namespace Eval360.Models
{
    public class User : IdentityUser
    {
        public string cin { get;set; }
        public string Nom { get; set; }
        public string preNom { get; set; }
        public string sexe { get; set; }
        public UserType UserType { get; set; }
        public DateTime dateEmbauche { get; set; }

        public int? idPoste { get; set; }
        public Poste? Poste { get; set; }

        public int? idSuperior { get; set; }
        public User? superior { get; set; }


      public User() { }
    }
}
