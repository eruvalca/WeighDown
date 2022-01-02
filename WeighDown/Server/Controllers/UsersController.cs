using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeighDown.Server.Services;
using WeighDown.Shared;

namespace WeighDown.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterVM registerModel)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await _usersService.Register(registerModel);

                if (registerResult.IsSuccess)
                {
                    //try to log user in
                    var loginModel = new LoginVM
                    {
                        Email = registerModel.Email,
                        Password = registerModel.Password
                    };

                    var loginResult = await _usersService.Login(loginModel);

                    if (loginResult.IsSuccess)
                    {
                        //return token for login
                        return Ok(loginResult);
                    }
                    else
                    {
                        //if login did not work just return register result
                        return Ok(registerResult);
                    }
                }
                else
                {
                    return BadRequest(registerResult);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Any())
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersService.Login(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest(new AuthResponse
                {
                    IsSuccess = false,
                    Messages = ModelState
                            .Where(m => m.Value.Errors.Any())
                            .Select(m => m.Value.Errors.ToString())
                            .ToList()
                });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<AuthResponse>> UpdateUser(WeighDownUser user)
        {
            var response = await _usersService.UpdateUser(user);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
