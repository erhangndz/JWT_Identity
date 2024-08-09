using JwtIdentity.API.Models;
using JwtIdentity.API.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtIdentity.API.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;
       
       

        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
         
           
        }

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricSecurityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<string> AddRoleAsync(CreateRoleDto createRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(createRoleDto.Email);
            if (user == null)
            {
                return $" {createRoleDto.Email} kullanıcısı sistemde kayıtlı değil.";
            }
            if (await _userManager.CheckPasswordAsync(user, createRoleDto.Password))
            {
                var roleExists = Enum.GetNames(typeof(Authorization.Roles))
                    .Any(x => x.ToLower() == createRoleDto.Role.ToLower());
                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(Authorization.Roles)).Cast<Authorization.Roles>()
                        .Where(x => x.ToString().ToLower() == createRoleDto.Role.ToLower())
                        .FirstOrDefault();

                    await _userManager.AddToRoleAsync(user, validRole.ToString());
                    return $" {createRoleDto.Role} rolü, {createRoleDto.Email} kullanıcısına atandı.";
                }
                return $" {createRoleDto.Role} rolü sistemde bulunamadı.";
            }
            return $" {user.Email} kullanıcı bilgileri yanlış, tekrar kontrol edin.";
        }

        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        {
            AuthenticationModel authenticationModel;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new AuthenticationModel { IsAuthenticated = false, Message = $"{model.Email} kullanıcısı sistemde kayıtlı değil." };

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

                authenticationModel = new AuthenticationModel
                {
                    IsAuthenticated = true,
                    Message = jwtSecurityToken.ToString(),
                    UserName = user.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = user.Email,
                };
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;
            }
            else
            {
                authenticationModel = new AuthenticationModel()
                {
                    IsAuthenticated = false,
                    Message = $"{user.Email} kullanıcı giriş bilgileri yanlış."
                };
            }


            return authenticationModel;
        }

        public async Task<string> RegisterAsync(CreateUserDto createUserDto)
        {
            var user = new AppUser
            {
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                Name = createUserDto.Name,
                Surname = createUserDto.Surname

            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (userWithSameEmail != null)
            {
                return $"{user.Email} kullanıcısı zaten sistemde kayıtlı";
            }

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Authorization.default_role.ToString());

            }
            return $"{user.UserName} kullanıcısı sisteme kaydedildi";

          


        }
    }
}
