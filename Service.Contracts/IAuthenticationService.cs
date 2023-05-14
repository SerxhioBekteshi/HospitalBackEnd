using Microsoft.AspNetCore.Identity;
using Shared.DTO;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> CreateUserAsync(UserRegisterDTO userRegister, string role);
    Task<TokenDTO> ValidateUserAndCreateToken(UserLoginDTO userLogin);
    Task<TokenDTO> RefreshToken(TokenDTO tokenDto);
    Task<TokenDTO> TwoStepVerification(TwoStepDTO twoFactorDto);
}