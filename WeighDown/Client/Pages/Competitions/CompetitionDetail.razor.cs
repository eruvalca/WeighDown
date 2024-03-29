﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WeighDown.Client.Services;
using WeighDown.Shared;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Pages.Competitions
{
    [Authorize]
    public partial class CompetitionDetail
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        private CompetitionsService CompetitionsService { get; set; }
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private ContestantsService ContestantsService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        [Parameter]
        public int CompetitionId { get; set; }

        private ClaimsPrincipal User { get; set; }
        private Competition Competition { get; set; }
        private WeighDownUser WeighDownUser { get; set; }
        private bool CanUserJoin { get; set; }
        private bool DisableJoin { get; set; }

        private WeighInDeadline NextWeighInDeadline { get; set; }
        private List<WeighInDeadline> RemainingWeighInDeadlines { get; set; }
        private Contestant UserContestant { get; set; }
        private bool HasUserWeighedIn { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = (await AuthenticationStateTask).User;

            WeighDownUser = await UsersService.GetUserDetails();
            Competition = await CompetitionsService.GetCompetition(CompetitionId);

            if (Competition.StartDate.ToLocalTime().Date <= DateTime.Today.Date
                && Competition.FirstPlacePrizeAmount == 0
                && Competition.SecondPlacePrizeAmount == 0
                && Competition.ThirdPlacePrizeAmount == 0)
            {
                Competition.ThirdPlacePrizeAmount = Competition.PlayInAmount;
                Competition.SecondPlacePrizeAmount = Competition.PlayInAmount + (decimal)Math.Floor((double)Competition.PlayInAmount * 0.666666667);
                Competition.FirstPlacePrizeAmount = (Competition.PlayInAmount * Competition.Contestants.Count) - (Competition.ThirdPlacePrizeAmount + Competition.SecondPlacePrizeAmount);

                await CompetitionsService.PutCompetition(CompetitionId, Competition);
                Competition = await CompetitionsService.GetCompetition(CompetitionId);
            }

            Competition.WeighInDeadlines.ForEach(w => w.DeadlineDate = w.DeadlineDate.ToLocalTime());
            NextWeighInDeadline = Competition.GetNextWeighInDeadline(DateTime.Today);

            if (NextWeighInDeadline != null)
            {
                RemainingWeighInDeadlines = Competition.GetRemainingWeighInDeadlines(NextWeighInDeadline.DeadlineDate.Date);
            }

            CanUserJoin = Competition.IsUserEligibleToJoin(WeighDownUser);

            UserContestant = Competition.Contestants.FirstOrDefault(c => c.WeighDownUserId == WeighDownUser.Id);

            if (UserContestant is not null && NextWeighInDeadline != null)
            {
                HasUserWeighedIn = UserContestant.WeightLogs.Any(w => w.MeasurementDate.ToLocalTime().Date == NextWeighInDeadline.DeadlineDate.ToLocalTime().Date);
            }
        }

        private async Task HandleContestantJoin()
        {
            DisableJoin = true;

            var contestant = new Contestant()
            {
                WeighDownUserId = WeighDownUser.Id,
                FirstName = WeighDownUser.FirstName,
                LastName = WeighDownUser.LastName,
                CompetitionId = Competition.CompetitionId
            };

            contestant = await ContestantsService.PostContestant(contestant);
            Competition = await CompetitionsService.GetCompetition(CompetitionId);

            CanUserJoin = Competition.IsUserEligibleToJoin(WeighDownUser);
            UserContestant = Competition.Contestants.FirstOrDefault(c => c.WeighDownUserId == WeighDownUser.Id);

            if (UserContestant is not null)
            {
                HasUserWeighedIn = UserContestant.WeightLogs.Any(w => w.MeasurementDate.ToLocalTime().Date == NextWeighInDeadline.DeadlineDate.ToLocalTime().Date);
            }

            DisableJoin = false;
        }

        private async Task HandleDelete()
        {
            await CompetitionsService.DeleteCompetition(CompetitionId);
            Navigation.NavigateTo("/");
        }
    }
}
