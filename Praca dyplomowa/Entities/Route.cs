using System;
using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Entities
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        // Nazwa drogi
        public String RouteName { get; set; }
        // Typ drogi
        public int RouteType { get; set; }
        // Długość/wysokość
        public int Length { get; set; }
        // Przewyższenie
        public int HeightDifference { get; set; }
        // Poziom realizacji  
        public int Accomplish { get; set; }
        // Typ skały
        public string Material { get; set; }

        // Typ skali
        public int Scale { get; set; }
        // Wycena drogi
        public String Rating { get; set; }
        // Liczba ringów
        public int Rings { get; set; }
        // Typ stanowiska zjazdowego
        public int DescentPosition { get; set; }

        // Miejsce w którym się znajduje
        public Place Place { get; set; }
        public int PlaceId { get; set; }
    }
}
