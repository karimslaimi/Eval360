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


        //todo add direction, poste, and superior


      public User() { }
    }
}
