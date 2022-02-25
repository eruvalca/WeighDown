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

        public bool IsCompetitionComplete()
        {
            if (DateTime.Now.Date < EndDate.ToLocalTime().Date)
            {
                return false;
            }

            if (Contestants.Any(c => !c.WeightLogs.Any(w => w.MeasurementDate.ToLocalTime().Date == EndDate.ToLocalTime().Date)))
            {
                return false;
            }

            return true;
        }

        public bool IsUserEligibleToJoin(WeighDownUser user)
        {
            var hasCompetitionStarted = StartDate.ToLocalTime().Date < DateTime.Today.Date;

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
                .Where(w => w.DeadlineDate.ToLocalTime().Date >= date.Date && w.IsActive)
                .OrderBy(w => w.DeadlineDate)
                .Take(1)
                .FirstOrDefault();
        }

        public List<WeighInDeadline> GetRemainingWeighInDeadlines(DateTime date)
        {
            return WeighInDeadlines
                .Where(w => w.DeadlineDate.ToLocalTime().Date > date.Date && w.IsActive)
                .ToList();
        }

        public List<ContestantResultSet> GetFinalResults()
        {
            var finalSet = new List<ContestantResultSet>();

            foreach (var contestant in Contestants)
            {
                var contestantResultSet = new ContestantResultSet
                {
                    Contestant = contestant,
                    PreviousWeightMeasurement = contestant.WeightLogs
                        .Where(w => w.MeasurementDate.ToLocalTime().Date == StartDate.ToLocalTime().Date)
                        .OrderByDescending(w => w.MeasurementDate.ToLocalTime())
                        .Take(1)
                        .FirstOrDefault()
                        .WeightMeasurement,
                    DeadlineWeightMeasurement = contestant.WeightLogs
                        .Where(w => w.MeasurementDate.ToLocalTime().Date == EndDate.ToLocalTime().Date)
                        .Take(1)
                        .FirstOrDefault()
                        .WeightMeasurement
                };

                finalSet.Add(contestantResultSet);
            }

            finalSet.OrderByDescending(d => d.PercentChange)
                .Take(1)
                .FirstOrDefault()
                .IsDeadlineWinner = true;

            return finalSet;
        }

        public Dictionary<WeighInDeadline, List<ContestantResultSet>> GetWeeklyResults()
        {
            var results = new Dictionary<WeighInDeadline, List<ContestantResultSet>>();
            var completeDeadlines = new List<WeighInDeadline>();

            var relevantDeadlines = WeighInDeadlines
                .Where(w => w.IsActive
                    && w.DeadlineDate.ToLocalTime().Date != StartDate.ToLocalTime().Date
                    && w.DeadlineDate.ToLocalTime().Date <= DateTime.Today.Date)
                .OrderBy(w => w.DeadlineDate.Date)
                .ToList();

            foreach (var deadline in relevantDeadlines)
            {
                var deadlineComplete = true;

                foreach (var contestant in Contestants)
                {
                    if (!contestant.WeightLogs.Any(w => w.MeasurementDate.ToLocalTime().Date == deadline.DeadlineDate.Date))
                    {
                        deadlineComplete = false;
                    }
                }

                if (deadlineComplete)
                {
                    completeDeadlines.Add(deadline);
                }
            }

            foreach (var deadline in completeDeadlines)
            {
                var deadlineSet = new List<ContestantResultSet>();

                foreach (var contestant in Contestants)
                {
                    var contestantResultSet = new ContestantResultSet
                    {
                        Contestant = contestant,
                        PreviousWeightMeasurement = contestant.WeightLogs
                            .Where(w => w.MeasurementDate.ToLocalTime().Date < deadline.DeadlineDate.ToLocalTime().Date)
                            .OrderByDescending(w => w.MeasurementDate.ToLocalTime())
                            .Take(1)
                            .FirstOrDefault()
                            .WeightMeasurement,
                        DeadlineWeightMeasurement = contestant.WeightLogs
                            .Where(w => w.MeasurementDate.ToLocalTime().Date == deadline.DeadlineDate.ToLocalTime().Date)
                            .Take(1)
                            .FirstOrDefault()
                            .WeightMeasurement
                    };

                    deadlineSet.Add(contestantResultSet);
                }

                deadlineSet.OrderByDescending(d => d.PercentChange)
                    .Take(1)
                    .FirstOrDefault()
                    .IsDeadlineWinner = true;

                results.Add(deadline, deadlineSet);
            }

            return results;
        }
    }
}
