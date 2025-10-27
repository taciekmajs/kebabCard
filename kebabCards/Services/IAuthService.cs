using kebabCards.Models;
using kebabCards.Models.Dtos;
using kebabCards.Utlis;
using System.Threading;
using System.Threading.Tasks;

namespace kebabCards.Services
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken ct = default);
        Task<ApiResponse> RegisterAsync(RegisterRequestDto registerDto, CancellationToken ct = default);
        Task<ApiResponse> GetAllUsersAsync(CancellationToken ct = default);
    }
}
