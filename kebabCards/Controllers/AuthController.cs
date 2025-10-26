using kebabCards.Data;
using kebabCards.Models.Dtos;
using kebabCards.Models;
using kebabCards.Utlis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kebabCards.Services;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace kebabCards.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string _secretKey;
        public AuthController(ApplicationDbContext context, IConfiguration configuration,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            User applicationUser = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            if (applicationUser == null)
            {
                _response.Result = new LoginResponseDto();
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Użytkownik o takim loginie nie istnieje.");
                return BadRequest(_response);
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(applicationUser, loginRequestDto.Password);
            if (!isPasswordValid)
            {
                _response.Result = new LoginResponseDto();
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ReturnMessage = "Nazwa użytkownika lub hasło jest niepoprawne";
                return BadRequest(_response);
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            var roles = _userManager.GetRolesAsync(applicationUser);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("name", applicationUser.Name),
                    new Claim("id", applicationUser.Id.ToString()),
                    //I assume that users have one role only.
                    new Claim(ClaimTypes.Role, roles.Result.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Name = applicationUser.Name,
                Token = tokenHandler.WriteToken(token)
            };

            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponseDto;
            return Ok(_response);
        }
        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterRequestDto registerDto)
        {
            User applicationUser = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == registerDto.UserName.ToLower());
            if (applicationUser != null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("User already exists.");
                return BadRequest(_response);
            }
            User newUser = new User()
            {
                UserName = registerDto.UserName,
                Name = registerDto.Name,
            };
            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(UserUtils.Role_Admin).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserUtils.Role_Admin));
                }
                if (!_roleManager.RoleExistsAsync(UserUtils.Role_User).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserUtils.Role_User));
                }
                if (registerDto.Role.ToLower() == UserUtils.Role_Admin.ToLower())
                {
                    await _userManager.AddToRoleAsync(newUser, UserUtils.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, UserUtils.Role_User);
                }
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            foreach (var err in result.Errors)
            {
                _response.Errors.Add(err.Description);
            }
            _response.ReturnMessage = String.Join(",", _response.Errors);
            return BadRequest(_response);
        }
        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<ApiResponse>> GetAllUsers()
        {
            try
            {
                _response.Result = _context.Users.ToList();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.ReturnMessage = "Poprawnie wczytano listę użytkowników";
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.ToString());
                _response.ReturnMessage = "Coś poszlo nie tak";
                _response.IsSuccess = false;
            }

            return Ok(_response);
        }
    }
}
