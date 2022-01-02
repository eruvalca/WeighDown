using Microsoft.AspNetCore.Components;
using WeighDown.Client.Services;
using WeighDown.Shared;

namespace WeighDown.Client.Pages.User
{
    public partial class Login
    {
        [Inject]
        private UsersService UsersService { get; set; }
        [Inject]
        private NavigationManager Navigation { get; set; }

        private LoginVM LoginVM { get; set; } = new LoginVM();

        private AuthResponse ServerResponse { get; set; }

        private bool ShowServerErrors { get; set; }
        private bool DisableSubmit { get; set; } = false;

        private async Task HandleSubmit()
        {
            DisableSubmit = true;
            ShowServerErrors = false;
            ServerResponse = await UsersService.Login(LoginVM);

            if (ServerResponse.IsSuccess)
            {
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
