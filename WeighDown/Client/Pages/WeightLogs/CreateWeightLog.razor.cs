using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WeighDown.Client.Services;
using WeighDown.Shared;
using WeighDown.Shared.Models;

namespace WeighDown.Client.Pages.WeightLogs
{
    [Authorize]
    public partial class CreateWeightLog
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private CompetitionsService CompetitionsService { get; set; }
        [Inject]
        private ContestantsService ContestantsService { get; set; }
        [Inject]
        private WeightLogsService WeightLogsService { get; set; }
        [Inject]
        private UploadService UploadService { get; set; }
        [Inject]
        private ComputerVisionService ComputerVisionService { get; set; }

        [Parameter]
        public int CompetitionId { get; set; }

        private WeighDownUser WeighDownUser { get; set; }
        private Competition Competition { get; set; } = new Competition();
        private Contestant UserContestant { get; set; }
        private WeightLog WeightLog { get; set; } = new WeightLog();
        private bool IsFormSubmitting { get; set; } = false;
        private List<decimal> ReadMeasurements { get; set; } = new List<decimal>();
        private bool IsOverrideMeasurement { get; set; } = false;
        private bool IsImageDataLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            WeighDownUser = await UsersService.GetUserDetails();
            Competition = await CompetitionsService.GetCompetition(CompetitionId);

            UserContestant = Competition.Contestants.FirstOrDefault(c => c.WeighDownUserId == WeighDownUser.Id);
            WeightLog.ContestantId = UserContestant.ContestantId;
            WeightLog.CompetitionId = Competition.CompetitionId;
        }

        private async Task HandleSubmit()
        {
            IsFormSubmitting = true;

            WeightLog.MeasurementDate = WeightLog.MeasurementDate.ToUniversalTime();

            WeightLog = await WeightLogsService.PostWeightLog(WeightLog);
            Navigation.NavigateTo($"competition-detail/{Competition.CompetitionId}");
        }

        private async Task HandleImageSelected(InputFileChangeEventArgs e)
        {
            IsImageDataLoading = true;

            WeightLog.ImageUrl = string.Empty;
            ReadMeasurements = new List<decimal>();
            WeightLog.WeightMeasurement = 0;
            WeightLog.RecognizedWeightMeasurement = 0;
            IsOverrideMeasurement = false;

            WeightLog.ImageUrl = await UploadService.UploadWeightLogImage(e.File);
            var reads = await ComputerVisionService.PostWeightLogImageData(new ComputerVisionDTO() { Url = WeightLog.ImageUrl });

            foreach (var read in reads)
            {
                if (decimal.TryParse(read.Replace(" ", "").Replace("-", ""), out var value))
                {
                    ReadMeasurements.Add(value);
                }
            }

            WeightLog.RecognizedWeightMeasurement = ReadMeasurements.FirstOrDefault();

            if (WeightLog.RecognizedWeightMeasurement == 0)
            {
                IsOverrideMeasurement = true;
            }
            else
            {
                WeightLog.WeightMeasurement = WeightLog.RecognizedWeightMeasurement;
            }

            IsImageDataLoading = false;
        }
    }
}
