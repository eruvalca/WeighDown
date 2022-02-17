using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Client.Services;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Shared.Competitions
{
    [Authorize]
    public partial class AllCompetitions
    {
        [Inject]
        private CompetitionsService CompetitionsService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private List<Competition> Competitions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCompetitions();
        }

        private async Task GetCompetitions()
        {
            var allCompetitions = await CompetitionsService.GetCompetitions();
            Competitions = allCompetitions
                .Where(c => c.EndDate.ToLocalTime().Date >= DateTime.Today.Date)
                .OrderByDescending(c => c.StartDate)
                .ToList();
        }
    }
}
