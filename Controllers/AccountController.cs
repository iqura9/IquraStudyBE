using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IquraStudyBE.Classes;
using IquraStudyBE.Context;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using IquraStudyBE.ViewModal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(
            MyDbContext context, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IConfiguration configuration,
            ITokenService tokenService
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }
        
        // POST: api/Account/Login
        /// <summary>
        /// Login    
        /// </summary>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim("id", user.Id),
                    new Claim("email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                
                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                    authClaims.Add(new Claim("role", role));
                }

                var token = _tokenService.CreateToken(authClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = refreshToken,
                    expiration = token.ValidTo
                });
            }
            return BadRequest("Invalid email or password");
        }
        
        // POST: api/Account/Register
        /// <summary>
        /// Register    
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Check if the email is already registered
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return BadRequest(ModelState);
            }
            // Check if the username is already taken
            var usernameExists = await _userManager.FindByNameAsync(model.UserName);
            if (usernameExists != null)
            {
                ModelState.AddModelError("Name", "Username already exists.");
                return BadRequest(ModelState);
            }
            // Create the new user
            User user = new()
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper(),
                UserName = model.UserName,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
            };
            
            
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }

            // If the user creation fails, add the errors to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        
        [HttpGet("GetMe")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var user = await GetUserByToken();
                return Ok(new { Message = "Success", Data = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        private async Task<UserInfo> GetUserByToken()
        {
            var userEmail = _tokenService.GetEmailFromToken();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return null;
            }
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var userRole = userRoles?.FirstOrDefault();
            
            return new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Image = user.Image,
                Description = user.Description,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Role = userRole,
            };
        }

        
    }
}