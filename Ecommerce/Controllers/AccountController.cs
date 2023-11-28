using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest("Role name cannot be empty.");

            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role already exists.");

            var newRole = new IdentityRole(roleName);

            var result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
                return Ok("Role created successfully.");
            else
                return BadRequest("Failed to create role.");
        }
        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser(string username, string password, string roleName)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(roleName))
                return BadRequest("Username, password, and role name cannot be empty.");

            var user = new IdentityUser { UserName = username };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (roleExists)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);

                    if (addToRoleResult.Succeeded)
                        return Ok("User created and added to role successfully.");
                    else
                        return BadRequest("Failed to add user to role.");
                }
                else
                {
                    return BadRequest($"Role '{roleName}' does not exist.");
                }
            }
            else
            {
                return BadRequest("Failed to create user.");
            }
        }
    }
}
