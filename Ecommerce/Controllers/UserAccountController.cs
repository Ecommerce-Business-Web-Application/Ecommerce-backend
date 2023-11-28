using EcommerceDB.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserAccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("create-role")]
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
        [Route("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserViewModel userView)
        {
            if ((string.IsNullOrEmpty(userView.Email) || string.IsNullOrEmpty(userView.Username) )|| string.IsNullOrEmpty(userView.Password) || string.IsNullOrEmpty(userView.roleName))
                return BadRequest("Username, password, and role name cannot be empty.");

            var user = new IdentityUser { UserName = userView.Username };
            var result = await _userManager.CreateAsync(user, userView.Password);

            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(userView.roleName);

                if (roleExists)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, userView.roleName);

                    if (addToRoleResult.Succeeded)
                        return Ok("User created and added to role successfully.");
                    else
                        return BadRequest("Failed to add user to role.");
                }
                else
                {
                    return BadRequest($"Role '{userView.roleName}' does not exist.");
                }
            }
            else
            {
                return BadRequest("Failed to create user.");
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenString = GenerateJWTToken(user);
                return Ok(new { Token = tokenString });
            }
            return Unauthorized("Invalid authentication request.");
        }

        private string GenerateJWTToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}