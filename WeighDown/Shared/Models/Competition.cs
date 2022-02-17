using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeighDown.Shared.Models
{
    public class Competition
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public decimal PlayInAmount { get; set; }
        public decimal ThirdPlacePrizeAmount { get; set; }
        public decimal SecondPlacePrizeAmount { get; set; }
        public decimal FirstPlacePrizeAmount { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);
        public int NumberOfWeeks { get; set; } = 1;
        public string CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }

        public List<Contestant> Contestants { get; set; }
        public List<WeighInDeadline> WeighInDeadlines { get; set; }

        public bool IsUserEligibleToJoin(WeighDownUser user)
        {
            var hasCompetitionStarted = StartDate.ToLocalTime().Date > DateTime.Today.Date;

            if (hasCompetitionStarted)
            {
                return false;
            }

            var hasCompetitionAnyContestants = Contestants.Any();

            if (hasCompetitionAnyContestants)
            {
                var isUserAlreadyContestant = Contestants.Any(c => c.WeighDownUserId == user.Id);

                if (isUserAlreadyContestant)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public WeighInDeadline GetNextWeighInDeadline(DateTime date)
        {
            return WeighInDeadlines
                .Where(w => w.DeadlineDate.ToLocalTime().Date >= date.Date)
                .OrderBy(w => w.DeadlineDate)
                .Take(1)
                .FirstOrDefault();
        }

        public List<WeighInDeadline> GetRemainingWeighInDeadlines(DateTime date)
        {
            return WeighInDeadlines
                .Where(w => w.DeadlineDate.ToLocalTime().Date > date.Date)
                .ToList();
        }
    }
}
