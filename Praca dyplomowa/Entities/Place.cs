using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Entities
{
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }
        // Nazwa Góry/Skały/SztucznejŚciany
        public String PlaceName { get; set; }
        // Południk
        public String Latitude { get; set; }
        // Równoleżnik
        public String Longitude { get; set; }
        // Typ miejsca Góra/Skała/SztucznaŚciana
        public int PlaceType { get; set; }

        // Region w którym się znajduje
        public Region Region { get; set; }
        public int RegionId { get; set; }
        public IList<Route> Routes { get; set; }
    }
}
