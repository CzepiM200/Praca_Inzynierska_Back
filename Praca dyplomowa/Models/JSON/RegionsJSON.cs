using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Praca_dyplomowa.Models
{
    public class RegionJSON
    {
        public int RegionId { get; set; }
        public String RegionName { get; set; }
    }

    public class PlaceJSON
    {
        public int PlaceId { get; set; }
        public String PlaceName { get; set; }
        public String Latitude { get; set; }
        public String Longitude { get; set; }
        public int PlaceType { get; set; }

        public RegionJSON BelongRegion { get; set; }
    }

    public class RouteJSON
    {
        public int RouteId { get; set; }
        public String RouteName { get; set; }
        public int RouteType { get; set; }
        public int Length { get; set; }
        public int HeightDifference { get; set; }
        public int Accomplish { get; set; }
        public string Material { get; set; }
        public int Scale { get; set; }
        public String Rating { get; set; }
        public int Rings { get; set; }
        public int DescentPosition { get; set; }

        public PlaceJSON BelongPlace { get; set; }
    }

    public class RemoveIdJSON
    {
        public int Id { get; set; }
    }
}
