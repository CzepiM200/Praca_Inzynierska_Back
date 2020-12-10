using System;
using System.ComponentModel.DataAnnotations;

namespace Praca_dyplomowa.Entities
{
    public class Training
    {
        [Key]
        public int TrainingId { get; set; }
        // Typ treningu
        public int TrainingType { get; set; }
        // Nazwa treningu
        public String TrainingName { get; set; }
        // Notatka do treningu
        public String TrainingDescription { get; set; }

        // Początek treningu
        public String StartTime { get; set; }
        // Początek treningu
        public String EndTime { get; set; }
        // Czas w sekundach
        public int ActivityTime { get; set; }
        // Dystans w metrach
        public int Distance { get; set; }

        // Użytkownik do którego należy
        public User User { get; set; }
        // Przebyta droga
        public Route Route { get; set; }
    }
}
