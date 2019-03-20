using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppRelacionamentos.Data;
using AppRelacionamentos.Dtos;
using AppRelacionamentos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppRelacionamentos.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> singnInManager;

        // Precisamos trazer tanto o UserManagger, como o SignInManager pra serem injetados aqui
        public AuthController(IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> singnInManager
            )
        {
            this.singnInManager = singnInManager;
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //var userToCreate = this.mapper.Map<User>(userForRegisterDto);

            var user = new User();

            user.UserName = userForRegisterDto.Username;

            var result = await this.userManager.CreateAsync(user, userForRegisterDto.Password);
            
            if (result.Succeeded) 
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            Console.WriteLine("AEW PORRA" + userForLoginDto.Password);
            // Agora vamos pegar o User, e temos maneiras distintas de pegá-lo.
            var user = await this.userManager.FindByNameAsync(userForLoginDto.Username);

            // Aqui vamos verificar o password, passamos o usuário, a senha pra testar
            // e um parâmetro no fim dizendo se é pra bloquear a conta caso ele erre o password
            var result = await this.singnInManager
                .CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await this.userManager.Users //.Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());

                // Quando tiver o mapper
                // var userToReturn = this.mapper.Map<UseForListDto>(appUser);

                return Ok(new
                {
                    token = GenerateJwtToken(appUser),
                    // user = userToReturn
                });
            }

           return Unauthorized();

        }

        // Vamos tirar do Login a responsabilidade de criar o JTW (JSON Web Token)
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}