using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Shared.Competitions
{
    [Authorize]
    public partial class CompetitionQuickDetailsCard
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public Competition Competition { get; set; }
    }
}
