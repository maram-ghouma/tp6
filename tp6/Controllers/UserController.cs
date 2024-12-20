using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using tp6.Models;
using tp6.Services;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;


    public UserController(IUserService userService,RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _roleManager = roleManager;
        _userManager = userManager;
    }


    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsersList();
        return Ok(users);
    }
    [HttpGet("roles")]
    public IActionResult GetAllRoles()
    
    {
        var roles =  _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }
    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        
        
    }

    // POST: api/User/{userId}/assign-role
    [HttpPost("{userId}/assign-role")]
    public async Task<IActionResult> AssignRoleToUser(string userId, [FromBody] string roleName)
    {
        
        
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} assigned to user {user.UserName}");
            }

            return BadRequest("Failed to assign role");
        
       
    }
    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole([FromBody] string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return BadRequest("Role name cannot be empty");
        }

        
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists)
        {
            return BadRequest($"Role '{roleName}' already exists.");
        }

        
        var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (roleResult.Succeeded)
        {
            return Ok($"Role '{roleName}' has been successfully created.");
        }

        return BadRequest("Failed to create the role. Please try again.");
    }


    // POST: api/User/{userId}/remove-role
    [HttpPost("{userId}/remove-role")]
    public async Task<IActionResult> RemoveRoleFromUser(string userId, [FromBody] string roleName)
    {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role {roleName} removed from user {user.UserName}");
            }

            return BadRequest("Failed to remove role");
        
       
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] JsonElement requestBody)
    {
       
        if (!requestBody.TryGetProperty("userName", out var userNameElement))
        {
            return BadRequest("UserName is missing.");
        }

        var userName = userNameElement.GetString();

        if (!requestBody.TryGetProperty("email", out var emailElement))
        {
            return BadRequest("Email is missing.");
        }

        var email = emailElement.GetString();

        if (!requestBody.TryGetProperty("password", out var passwordElement))
        {
            return BadRequest("Password is missing.");
        }

        var password = passwordElement.GetString();

        
        var newUser = new ApplicationUser
        {
            UserName = userName,
            Email = email
        };

        
        var result = await _userManager.CreateAsync(newUser, password);
        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }




}
