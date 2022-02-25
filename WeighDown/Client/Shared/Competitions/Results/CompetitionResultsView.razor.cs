using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Shared;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Shared.Competitions.Results
{
    [Authorize]
    public partial class CompetitionResultsView
    {
        [Parameter]
        public Competition Competition { get; set; }

        private Dictionary<WeighInDeadline, List<ContestantResultSet>> CompetitionResults { get; set; }
        private KeyValuePair<WeighInDeadline, List<ContestantResultSet>> MostRecentResultSet { get; set; }
        private List<ContestantResultSet> FinalResults { get; set; }
        private bool IsCompetitionComplete { get; set; }

        protected override void OnInitialized()
        {
            CompetitionResults = Competition.GetWeeklyResults();

            if (CompetitionResults is not null && CompetitionResults.Any())
            {
                MostRecentResultSet = CompetitionResults
                    .Where(c => c.Key.IsActive && c.Key.DeadlineDate.ToLocalTime().Date != Competition.EndDate.ToLocalTime().Date)
                    .OrderByDescending(c => c.Key.DeadlineDate.ToLocalTime().Date)
                    .Take(1)
                    .FirstOrDefault();
            }

            IsCompetitionComplete = Competition.IsCompetitionComplete();

            if (IsCompetitionComplete)
            {
                FinalResults = Competition.GetFinalResults();
            }
        }
    }
}
