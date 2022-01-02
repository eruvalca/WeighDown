using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using WeighDown.Client.Providers;
using WeighDown.Client.Utilities;
using WeighDown.Shared;

namespace WeighDown.Client.Services
{
    public class UsersService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public string ClaimType { get; private set; }

        public UsersService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponse> Register(RegisterVM registerViewModel)
        {
            var response = await _client.PostAsJsonAsync("users/register", registerViewModel);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.IsSuccess)
            {
                if (!String.IsNullOrEmpty(authResult.Token))
                {
                    await _localStorage.SetItemAsync("authToken", authResult.Token);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                    ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
                }
            }

            return authResult;
        }

        public async Task<AuthResponse> Login(LoginVM loginViewModel)
        {
            var response = await _client.PostAsJsonAsync("users/login", loginViewModel);
            var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (authResult.IsSuccess)
            {
                await _localStorage.SetItemAsync("authToken", authResult.Token);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
            }

            return authResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _client.DefaultRequestHeaders.Authorization = null;
            ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<AuthResponse> UpdateUser(WeighDownUser user)
        {
            var response = await _client.PutAsJsonAsync("users/updateBrawler", user);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<WeighDownUser> GetUserDetails()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            var claims = JwtParser.ParseClaimsFromJwt(token);

            var user = new WeighDownUser
            {
                Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value,
                LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value
            };

            return user;
        }
    }
}