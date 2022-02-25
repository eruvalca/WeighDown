using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Shared;
using WeighDown.Shared.Models;
using System.Linq;

namespace WeighDown.Client.Shared.Competitions.Results
{
    [Authorize]
    public partial class PastResultCard
    {
        [Parameter]
        public KeyValuePair<WeighInDeadline, List<ContestantResultSet>> Results { get; set; }
    }
}
