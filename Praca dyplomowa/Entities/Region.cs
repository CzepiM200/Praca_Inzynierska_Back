using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Entities
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        // Nazwa regionu
        public String RegionName { get; set; }

        // Użytkownik do którego należy
        public User User { get; set; }
        public IList<Place> Places { get; set; }
    }
}
