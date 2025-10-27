using System.Net;
using kebabCards.Models.Dtos;
using kebabCards.Services;
using kebabCards.Utlis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kebabCards.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto, CancellationToken ct)
        {
            var resp = await _auth.LoginAsync(loginRequestDto, ct);
            return StatusCode((int)resp.StatusCode, resp);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto registerDto, CancellationToken ct)
        {
            var resp = await _auth.RegisterAsync(registerDto, ct);
            return StatusCode((int)resp.StatusCode, resp);
        }

        [Authorize(Roles = UserUtils.Role_Admin)]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers(CancellationToken ct)
        {
            var resp = await _auth.GetAllUsersAsync(ct);
            return StatusCode((int)resp.StatusCode, resp);
        }
    }
}