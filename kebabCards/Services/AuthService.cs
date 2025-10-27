using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kebabCards.Data;
using kebabCards.Models;
using kebabCards.Models.Dtos;
using kebabCards.Utlis;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace kebabCards.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _secretKey;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ApiResponse> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken ct = default)
        {
            var response = new ApiResponse();

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower(), ct);

            if (applicationUser == null)
            {
                response.Result = new LoginResponseDto();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors.Add("Użytkownik o takim loginie nie istnieje.");
                return response;
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(applicationUser, loginRequestDto.Password);
            if (!isPasswordValid)
            {
                response.Result = new LoginResponseDto();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ReturnMessage = "Nazwa użytkownika lub hasło jest niepoprawne";
                return response;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            var roles = await _userManager.GetRolesAsync(applicationUser);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("name", applicationUser.Name ?? string.Empty),
                    new Claim("id", applicationUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

            var loginResponseDto = new LoginResponseDto()
            {
                Name = applicationUser.Name,
                Token = tokenHandler.WriteToken(token)
            };

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = loginResponseDto;
            return response;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequestDto registerDto, CancellationToken ct = default)
        {
            var response = new ApiResponse();

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == registerDto.UserName.ToLower(), ct);

            if (applicationUser != null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors.Add("User already exists.");
                return response;
            }

            var newUser = new User()
            {
                UserName = registerDto.UserName,
                Name = registerDto.Name,
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(UserUtils.Role_Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserUtils.Role_Admin));

                if (!await _roleManager.RoleExistsAsync(UserUtils.Role_User))
                    await _roleManager.CreateAsync(new IdentityRole(UserUtils.Role_User));

                if (registerDto.Role.ToLower() == UserUtils.Role_Admin.ToLower())
                    await _userManager.AddToRoleAsync(newUser, UserUtils.Role_Admin);
                else
                    await _userManager.AddToRoleAsync(newUser, UserUtils.Role_User);

                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return response;
            }

            foreach (var err in result.Errors)
                response.Errors.Add(err.Description);

            response.ReturnMessage = string.Join(",", response.Errors);
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            return response;
        }

        public async Task<ApiResponse> GetAllUsersAsync(CancellationToken ct = default)
        {
            var response = new ApiResponse();

            try
            {
                response.Result = await _context.Users.ToListAsync(ct);
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.ReturnMessage = "Poprawnie wczytano listę użytkowników";
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.ReturnMessage = "Coś poszlo nie tak";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
