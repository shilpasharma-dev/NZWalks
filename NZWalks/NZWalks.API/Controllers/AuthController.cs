using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostitories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                //assign roles to user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Length > 0)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User is registered. Please Login");
                    }
                }
            }
            return BadRequest("Something went wrong.");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var identityUser = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if(identityUser != null)
            {
                var isPasswordValid = await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);
                if(isPasswordValid)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    if(roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                        
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }

                    return Ok("Login Successful.");
                }
            }
            return BadRequest("Invalid username or password");
        }
    }
}
