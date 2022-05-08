using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WeighDown.Client.Services;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Pages.Competitions
{
    [Authorize]
    public partial class EditCompetition
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private CompetitionsService CompetitionsService { get; set; }

        [Parameter]
        public int CompetitionId { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private DateTime _startDate;
        private DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                HandleStartDateChange();
            }
        }
        private int _numberOfWeeks;
        private int NumberOfWeeks
        {
            get { return _numberOfWeeks; }
            set
            {
                _numberOfWeeks = value;
                HandleWeekChange();
            }
        }
        private string UserId { get; set; }
        private string Username { get; set; }
        private Competition Competition { get; set; }
        private bool IsFormSubmitting { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                UserId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
                Username = user.FindFirst(c => c.Type == ClaimTypes.GivenName).Value;
            }

            Competition = await CompetitionsService.GetCompetition(CompetitionId);

            _startDate = Competition.StartDate.ToLocalTime();
            _numberOfWeeks = Competition.NumberOfWeeks;

            Competition.WeighInDeadlines.ForEach(w => w.DeadlineDate = w.DeadlineDate.ToLocalTime());
        }

        private void HandleStartDateChange()
        {
            Competition.StartDate = StartDate;
            Competition.EndDate = Competition.StartDate.AddDays(Competition.NumberOfWeeks * 7);

            Competition.WeighInDeadlines = new List<WeighInDeadline>
            {
                new WeighInDeadline()
                {
                    DeadlineDate = Competition.StartDate,
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                },
                new WeighInDeadline()
                {
                    DeadlineDate = Competition.EndDate,
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                }
            };

            for (int i = 1; i < Competition.NumberOfWeeks; i++)
            {
                Competition.WeighInDeadlines.Add(new WeighInDeadline()
                {
                    DeadlineDate = Competition.StartDate.AddDays(i * 7),
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                });
            }
        }

        private void HandleWeekChange()
        {
            Competition.NumberOfWeeks = NumberOfWeeks;
            Competition.EndDate = Competition.StartDate.AddDays(NumberOfWeeks * 7);

            Competition.WeighInDeadlines = new List<WeighInDeadline>
            {
                new WeighInDeadline()
                {
                    DeadlineDate = Competition.StartDate,
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                },
                new WeighInDeadline()
                {
                    DeadlineDate = Competition.EndDate,
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                }
            };

            for (int i = 1; i < NumberOfWeeks; i++)
            {
                Competition.WeighInDeadlines.Add(new WeighInDeadline()
                {
                    DeadlineDate = Competition.StartDate.AddDays(i * 7),
                    IsActive = true,
                    CompetitionId = Competition.CompetitionId
                });
            }
        }

        private async Task HandleSubmit()
        {
            IsFormSubmitting = true;

            Competition.CreateDate = Competition.CreateDate.ToUniversalTime();
            Competition.StartDate = Competition.StartDate.ToUniversalTime();
            Competition.EndDate = Competition.EndDate.ToUniversalTime();

            Competition.WeighInDeadlines.ForEach(w => w.DeadlineDate = w.DeadlineDate.ToUniversalTime());

            await CompetitionsService.PutCompetition(CompetitionId, Competition);
            Navigation.NavigateTo($"/competition-detail/{Competition.CompetitionId}");
        }
    }
}
