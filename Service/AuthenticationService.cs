using AutoMapper;
using Cryptography;
using EmailService;
using Entities.Configuration;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;
using Shared.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IEmailSender _emailSender;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ICryptoUtils _cryptoUtils;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly DefaultConfiguration _defaultConfig;

    public AuthenticationService(
          ILoggerManager logger
        , IMapper mapper
        , UserManager<ApplicationUser> userManager
        , IOptions<JwtConfiguration> configuration
        , IRepositoryManager repositoryManager
        , SignInManager<ApplicationUser> signInManager
        , IEmailSender emailSender
        , ICryptoUtils cryptoUtils
        ,DefaultConfiguration defaultConfiguration

        )
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
        _emailSender = emailSender;
        _repositoryManager = repositoryManager;
        _signInManager = signInManager;
        _cryptoUtils = cryptoUtils;
        _defaultConfig = defaultConfiguration;
    }

    public async Task<IdentityResult> CreateUserAsync(UserRegisterDTO userRegister, string role)
    {
        try
        {
            var user = _mapper.Map<ApplicationUser>(userRegister);
            user.UserName = userRegister.Email;
            user.DateCreated = DateTime.UtcNow;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                //await _userManager.SetTwoFactorEnabledAsync(user, true);
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(CreateUserAsync), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<TokenDTO> ValidateUserAndCreateToken(UserLoginDTO userLogin)
    {
        try
        {
            ApplicationUser currentUser = await _userManager.FindByNameAsync(userLogin.Email);

            if (currentUser is null)
                throw new BadRequestException($"Përdoruesi me email {userLogin.Email} nuk ekziston.");

            //var validateUser = await _signInManager.PasswordSignInAsync(currentUser, userLogin.Password, false, lockoutOnFailure: true);
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            var password = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (currentUser.EmailConfirmed is true)
            {
                if (!password)
                {
                    //if (validateUser.IsLockedOut)
                    //{
                    //    await HandleLockout(currentUser);
                    //    throw new BadRequestException("Përpjekje të njëpasnjëshme për login! Provoni mbas disa minutash!");
                    //}
                    //else if (validateUser.RequiresTwoFactor)
                    //{
                    //    await SendTwoFactorVerificationCodeEmail(currentUser, currentUser.Email, currentUser.FirstName, currentUser.LastName);
                    //    throw new BadRequestException("Kodi i verifikimit u dërgua tek e-mail dhe numri juaj!");
                    //}
                    //else
                    //{
                        _logger.LogWarn(string.Format("{0}: Authentication failed.", nameof(ValidateUserAndCreateToken)));
                        throw new BadRequestException("E-mail ose fjalëkalimi është i pasaktë!");
                    //}
                }
                else
                {
                    var tokenDto = await CreateToken(currentUser, true);
                    return tokenDto;
                }
            }
            else
            {
                throw new BadRequestException("Ju nuk mund të logoheni nëse nuk keni konfirmuar ende e-mail tuaj!");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(ValidateUserAndCreateToken), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<TokenDTO> RefreshToken(TokenDTO tokenDto)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

            var currentUserEmail = principal.Claims.Where(x => x.Type == "Email").FirstOrDefault();
            if (currentUserEmail is null)
                throw new BadRequestException("Invalid client request. The tokenDto has some invalid values.");

            var user = await _userManager.FindByEmailAsync(currentUserEmail.Value);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new BadRequestException("Invalid client request. The tokenDto has some invalid values.");

            return await CreateToken(user, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(RefreshToken), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }



    public async Task<TokenDTO> TwoStepVerification(TwoStepDTO twoStepDto)
    {
        try
        {
            var userCheck = await _userManager.FindByEmailAsync(twoStepDto.Email);
            if (userCheck is null)
                throw new NotFoundException(string.Format("Përdoruesi me e-mail {0} nuk u gjet!", twoStepDto.Email));

            var result = await _userManager.VerifyTwoFactorTokenAsync(userCheck, "Email", twoStepDto.TwoFactorCode);

            await _userManager.UpdateAsync(userCheck);
            var tokenDto = await CreateToken(userCheck, true);
            return tokenDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(TwoStepVerification), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    private async Task SendRegistrationEmailAndSms(string userEmail, string firstName, string lastName, string phoneNumber)
    {
        var welcomeEmailTemplate = await _repositoryManager.EmailTemplateRepository.GetRecordByCodeAsync(EmailTemplateCode.Welcome);

        if (welcomeEmailTemplate is null)
            _logger.LogError($"Email/Sms Template me kod: {EmailTemplateCode.Welcome} nuk u gjet!");
        else
        {
            welcomeEmailTemplate.Body = welcomeEmailTemplate.Body.Replace("{fullName}", $"{firstName} {lastName}");
            welcomeEmailTemplate.Body = welcomeEmailTemplate.Body.Replace("{url}", _defaultConfig.ApplicationUrl);

            var message = new Message(new string[] { userEmail }, welcomeEmailTemplate.Subject, welcomeEmailTemplate.Body);
            await _emailSender.SendEmailAsync(message);

            //if (!string.IsNullOrWhiteSpace(phoneNumber))
            //{
            //    var smsMessage = new SmsMessage(phoneNumber, "Mirë se vjen në Finalb!");
            //    await _smsSender.SendSmsAsync(_defaultConfig.UseTokyDigitalProvider, smsMessage);
            //}
        }
    }


    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        int refreshTokenExpire = Convert.ToInt32(_jwtConfiguration.RefreshTokenExpire);
        int tokenExpire = Convert.ToInt32(_jwtConfiguration.Expires);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ClockSkew = TimeSpan.FromMinutes(refreshTokenExpire - tokenExpire),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey)),
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null)
            throw new SecurityTokenException("Token jo i vlefshëm!");

        return principal;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private async Task<ClaimsIdentity> GetClaims(ApplicationUser currentUser, string tokenHash)
    {
        var claims = new List<Claim>
             {
                new Claim("Id", currentUser.Id.ToString()),
                new Claim("Email", currentUser.Email),
                new Claim("FirstName", currentUser.FirstName),
                new Claim("LastName", currentUser.LastName),
                new Claim("TokenHash", tokenHash)
            };

        var roles = await _userManager.GetRolesAsync(currentUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new ClaimsIdentity(claims);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
    }

    private async Task<TokenDTO> CreateToken(ApplicationUser? currentUser, bool populateExp)
    {
        try
        {
            if (currentUser is not null)
            {
                var signingCredentials = GetSigningCredentials();
                var tokenHash = _cryptoUtils.Encrypt($"{currentUser.Id}{currentUser.Email}{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}");
                var claims = await GetClaims(currentUser, tokenHash);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    NotBefore = DateTime.UtcNow,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                    SigningCredentials = signingCredentials,
                    Audience = _jwtConfiguration.ValidAudience,
                    Issuer = _jwtConfiguration.ValidIssuer
                };

                var refreshToken = GenerateRefreshToken();
                currentUser.RefreshToken = refreshToken;
                currentUser.TokenHash = tokenHash;

                if (populateExp)
                    currentUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.RefreshTokenExpire));

                await _userManager.UpdateAsync(currentUser);

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);

                return new TokenDTO(accessToken, refreshToken);
            }
            return new TokenDTO("", "");
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(CreateToken), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

}
