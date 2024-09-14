using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<ApplicationUser> userManager,ITokenRepository obj)
        {
            _userManger = userManager;
            _tokenRepository = obj;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto obj)
        {
            var identityUser = new ApplicationUser
            {
                UserName = obj.UserName,
                Email = obj.UserName
            };
            var identityResult = await _userManger.CreateAsync(identityUser, obj.Password);
            if (identityResult.Succeeded)
            {
                //add role to this user 
                identityResult =await _userManger.AddToRolesAsync(identityUser, obj.Roles);
                if (identityResult.Succeeded) { return Ok("user registerd , please login"); }
            }
            return BadRequest("somethign went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto obj)
        {
            var user = await _userManger.FindByEmailAsync(obj.Username);
            if (user != null)
            {
                var checkPassResult = await _userManger.CheckPasswordAsync(user, obj.Password);
                if (checkPassResult)
                {
                    // Get roles for this user
                    var roles = await _userManger.GetRolesAsync(user);
                    if (roles != null)
                    {
                        // Create JWT token
                        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());

                        // Generate and set refresh token
                        var refreshToken = _tokenRepository.GenerateRefreshToken();
                        _tokenRepository.SetRefreshToken(refreshToken);

                        // Update user with the new refresh token details
                        user.RefreshToken = refreshToken.Token;
                        user.TokenCreated = refreshToken.Created;
                        user.TokenExpires = refreshToken.Expires;

                        await _userManger.UpdateAsync(user);

                        return Ok(new { Token = jwtToken, RefreshToken = refreshToken.Token });
                    }
                }
            }
            return BadRequest("Invalid login attempt.");
        }
    }
}
