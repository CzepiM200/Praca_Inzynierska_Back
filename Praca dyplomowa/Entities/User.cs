using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        // Id użytkownika w Google
        public string UserGoogleId { get; set; }
        // Imię użytkownika
        public string FirstName { get; set; }
        // Nazwisko użytkownika
        public string LastName { get; set; }
        // Nick użytkownika
        public string UserName { get; set; }
        // Email uzytkownika
        [Required]
        public string Email { get; set; }
        // Hash hasła
        public byte[] PasswordHash { get; set; }
        // Ciąg zakłucający hash hasła
        public byte[] PasswordSalt { get; set; }

        public IList<Region> UserRegions { get; set; }
        public IList<Training> UserTrainings { get; set; }
    }
}
