using Keeper.Common.Enums;
using Keeper.Common.InnerException;
using Keeper.Common.OtherModels;
using Keeper.Common.ViewModels;
using Keeper.Context.Model;
using Keeper.Repos.Interfaces;
using Keeper.Repos.Repositories.Interfaces;
using Keeper.Services.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crypto = BCrypt.Net.BCrypt;

namespace Keeper.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IMailService _mail;
        public AccountService(IAccountRepo accountRepo, IConfiguration configuration, IUserRepo userRepo, IMailService mail)
        {
            _accountRepo = accountRepo;
            _configuration = configuration;
            _userRepo = userRepo;
            _mail = mail;
        }
        public async Task<bool> RegisterAsync(RegisterModel register)
        {
            UserModel userModel = new()
            {
                UserName = register.UserName,
                Email = register.Email,
                Password = register.Password,
                Mobile = register.Mobile,
                CreatedOn = DateTime.Now,
                UpdateOn = null
            };
            var user = await _userRepo.GetByEmailAsync(register.Email);
            if (user != null)
            {
                throw new InnerException("Email already exists", StatusType.EMAIL_EXISTS);
            }
            userModel.Password = Crypto.HashPassword(register.Password);
            await _accountRepo.RegisterAsync(userModel);
            return true;
        }
        public async Task<TokenModel> LoginAsync(LoginModel login)
        {
            UserModel user = await _userRepo.GetByEmailAsync(login.Email) ??
            throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND);
            
            if (!Crypto.Verify(login.Password, user.Password))
                throw new InnerException("Password does not match", StatusType.PASSWORD_NOT_MATCHED);
            return new TokenModel()
            {
                Token = GenerateToken(user)
            };
        }
        public async Task<string> GetOTP(string email)
        {
            string otp = OtpGenerator;
            await _mail.SendEmailAsync(new MailModel
            {
                Category = MailCategory.OTP,
                From = "",
                To = email,
                Subject = "Email Verification",
                Message = otp
            });
            return otp;
        }
        public async Task<bool> UpdatePasswordAsync(PasswordResetModel resetModel)
        {
            UserModel user = await _userRepo.GetByEmailAsync(resetModel.Email) ?? throw new InnerException("Email is not registered", StatusType.EMAIL_NOT_FOUND);
            user.Password = Crypto.HashPassword(resetModel.Password);
            await _accountRepo.UpdatePasswordAsync(user);
            return true;
        }
        private string GenerateToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string OtpGenerator
        {
            get
            {
                const string firstCharacter = "123456789";
                const string restCharacters = "0123456789";
                const int otpLength = 6;
                Random random = new();
                string otp = new string(Enumerable.Repeat(restCharacters, otpLength - 1)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                otp = firstCharacter[random.Next(firstCharacter.Length)] + otp;
                return otp;
            }
        }
    }
}
