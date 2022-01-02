using Microsoft.AspNetCore.Components;

namespace WeighDown.Client.Shared.Auth
{
    public partial class RedirectToLogin
    {
        [Inject]
        private NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo("/user/login");
        }
    }
}
