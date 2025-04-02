using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightLossPlannerAPI.Models
{
    public class DailyCaloriesSummary
    {
        public DateTime Date { get; set; }
        public int CaloriesEaten { get; set; }
        public int CaloriesBurned { get; set; }
    }
}