using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tp6.Models;

namespace tp6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JWTBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<ApplicationUser> userManager;  // Use ApplicationUser instead of IdentityUser

        public AuthController(IOptions<JWTBearerTokenSettings> jwtTokenOptions,
                              UserManager<ApplicationUser> userManager)  // Inject UserManager<ApplicationUser>
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            this.userManager = userManager;
        }

        // Register method to create a new user
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCredentials userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new
                {
                    Message = "User Registration Failed"
                });
            }

            var applicationUser = new ApplicationUser()
            {
                UserName = userDetails.username,
                Email = userDetails.Email
            };

            var result = await userManager.CreateAsync(applicationUser, userDetails.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }
                return new BadRequestObjectResult(new
                {
                    Message = "User Registration Failed",
                    Errors = dictionary
                });
            }
            var roleResult = await userManager.AddToRoleAsync(applicationUser, "User");
            if (!roleResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to assign role" });
            }
            return Ok(new { Message = "User Registration Successful" });
        }

        // Login method to authenticate and generate a JWT token
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            ApplicationUser applicationUser;

            if (!ModelState.IsValid || credentials == null || (applicationUser = await ValidateUser(credentials)) == null)
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Login failed"
                });
            }

            var token = GenerateToken(applicationUser);
            return Ok(new { Token = token, Message = "Success" });
        }

        // Logout method
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Implement logout logic if necessary (invalidate token if needed)
            return Ok(new { Token = "", Message = "Logged Out" });
        }

        // Method to validate user credentials
        private async Task<ApplicationUser> ValidateUser(LoginCredentials credentials)
        {
            var applicationUser = await userManager.FindByNameAsync(credentials.Username);
            if (applicationUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : applicationUser;
            }
            return null;
        }

        // Method to generate JWT token
        private object GenerateToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, applicationUser.UserName),
                    new Claim(ClaimTypes.Email, applicationUser.Email)
                }),
                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpireTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
