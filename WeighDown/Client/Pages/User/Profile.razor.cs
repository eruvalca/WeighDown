using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using WeighDown.Client.Services;
using WeighDown.Shared;

namespace WeighDown.Client.Pages.User
{
    [Authorize]
    public partial class Profile
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private WeighDownUser User { get; set; }
        private AuthResponse ServerResponse { get; set; }
        private bool ShowServerErrors { get; set; }
        private bool DisableSubmit { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            User = await UsersService.GetUserDetails();
        }

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;
            ServerResponse = await UsersService.UpdateUser(User);

            if (ServerResponse.IsSuccess)
            {
                await UsersService.Logout();
                Navigation.NavigateTo("/");
            }
            else
            {
                ShowServerErrors = true;
                DisableSubmit = false;
            }
        }
    }
}
