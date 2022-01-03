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

        private List<Competition> Competitions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCompetitions();
        }

        private async Task GetCompetitions()
        {
            Competitions = await CompetitionsService.GetCompetitions();
        }
    }
}
