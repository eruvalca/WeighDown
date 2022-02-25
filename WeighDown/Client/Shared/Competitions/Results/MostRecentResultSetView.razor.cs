using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Shared;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Shared.Competitions.Results
{
    [Authorize]
    public partial class MostRecentResultSetView
    {
        [Parameter]
        public KeyValuePair<WeighInDeadline, List<ContestantResultSet>> MostRecentResultSet { get; set; }
    }
}
