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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JWTSettings _jwtSettings;



        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
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

        public async Task<bool> Login(TokenRequestModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }
          
               var result =  await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if(result.Succeeded)
            {
               var token =  await CreateJwtToken(user);
                return true;
            }

            return false;
               
                
            
            


        
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

        public async Task<string> GetAccessToken(AppUser appUser)
        {
            var token = await CreateJwtToken(appUser);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
