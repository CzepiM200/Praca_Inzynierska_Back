using Praca_dyplomowa.Entities;
using System;

namespace Praca_dyplomowa.Models
{
    public class TrainingJSON
    {
        public int TrainingId { get; set; }
        public int TrainingType { get; set; }
        public String TrainingName { get; set; }
        public String TrainingDescription { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int ActivityTime { get; set; }
        public int Distance { get; set; }
        public Route Route { get; set; }
    }

    public class EditTrainingJSON
    {
        public int TrainingId { get; set; }
        public int TrainingType { get; set; }
        public String TrainingName { get; set; }
        public String TrainingDescription { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int ActivityTime { get; set; }
        public int Distance { get; set; }
    }

    public class NewTrainingJSON
    {
        public String TrainingName { get; set; }
        public String TrainingDescription { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public int ActivityTime { get; set; }
        public int Distance { get; set; }
        public int RouteId { get; set; }
    }
}
