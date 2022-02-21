using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeighDown.Shared.Models;

namespace WeighDown.Shared
{
    public class ContestantResultSet
    {
        public Contestant Contestant { get; set; }
        public decimal PreviousWeightMeasurement { get; set; }
        public decimal DeadlineWeightMeasurement { get; set; }
        public decimal PercentChange => (PreviousWeightMeasurement - DeadlineWeightMeasurement) / PreviousWeightMeasurement;
        public bool IsDeadlineWinner { get; set; }
    }
}
