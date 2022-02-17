using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeighDown.Shared;

namespace WeighDown.Server.Services
{
    public class UsersService
    {
        private readonly UserManager<WeighDownUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly SymmetricKeyService _symmetricKeyService;

        public UsersService(UserManager<WeighDownUser> userManager, IConfiguration configuration, SymmetricKeyService symmetricKeyService, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _configuration = configuration;
            _symmetricKeyService = symmetricKeyService;
            _env = env;
        }

        public async Task<AuthResponse> Register(RegisterVM model)
        {
            var user = new WeighDownUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "General");

                return new AuthResponse
                {
                    IsSuccess = true,
                    Messages = new List<string> { "User registered successfully" }
                };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };
            }
        }

        public async Task<AuthResponse> Login(LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "User does not exist" }
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "Invalid Password" }
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var symmetricKey = _symmetricKeyService.GetSymmetricKey();
            string issuer;
            string audience;

            if (_env.EnvironmentName == Environments.Development)
            {
                issuer = "https://localhost:7018/";
                audience = "https://localhost:7018/";
            }
            else
            {
                issuer = "https://weighdown.azurewebsites.net/";
                audience = "https://weighdown.azurewebsites.net/";
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse
            {
                IsSuccess = true,
                Token = tokenString
            };
        }

        public async Task<List<WeighDownUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<WeighDownUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<WeighDownUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AuthResponse> UpdateUser(WeighDownUser user)
        {
            var thisUser = await GetUserByEmail(user.Email);

            thisUser.FirstName = user.FirstName;
            thisUser.LastName = user.LastName;

            var result = await _userManager.UpdateAsync(thisUser);

            if (result.Succeeded)
            {
                return new AuthResponse { IsSuccess = true };
            }
            else
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Messages = new List<string> { "There was an unexpected error updating the profile. Please try again." }
                };
            }
        }
    }
}
