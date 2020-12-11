using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Models
{
    public class UserIdModel
    {
        [Required]
        public int userId { get; set; }
    }
}
