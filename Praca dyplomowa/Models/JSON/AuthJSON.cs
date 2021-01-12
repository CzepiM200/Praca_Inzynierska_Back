using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class GoogleAuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string GoogleId { get; set; }

        [Required]
        public string GoogleToken { get; set; }
    }

    public class GoogleResponseModel
    {
        public bool Status { get; set; }
        public string Email { get; set; }
        public string Sub { get; set; }
        public string Given_name { get; set; }
        public string Family_name { get; set; }
    }

    public class GoogleTokenModel
    {
        public string Email { get; set; }
        public string GoogleToken { get; set; }
        public string GoogleId { get; set; }
    }
}



