using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppRelacionamentos.Data;
using AppRelacionamentos.Dtos;
using AppRelacionamentos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppRelacionamentos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;

        //(***) Injetando a configuração no nosso controller
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }

        // Como teremos tanto o login como registro pro post, precisamos especificar dentro o que virá
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate request

            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await this.repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username alredy exists.");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await this.repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        // Criamos também um DTO
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await this.repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // Agora faremos um token que será retornado ao usuário
            // Nosso token conterá 2 bits de informações sobre o usuário
            // Terá o ID do usuário e o username
            // Nós podemos colocar algumas informações aqui porque quando o server receber o token
            // Ele pode olhar dentro dele, mas não precisará ir no banco olhar o username ou ID
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // Nós precisamos guardar essa chave dentro do AppSettings, porque nós vamos usá-la em
            // alguns outros lugares. E nós precisamos guardá-la, quase do mesmo jeito que guardamos a nossa
            // ConnectionString. 
            // Nós precisaremos injetar a nossa configuração no nosso controller. (***)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));
            // Agora precisamos criar isso dentro do appsettings.json

            // Agora utilizaremos a chave pra gerar as credenciais, e escolher o algortimo
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Agora que temos as credenciais de registro, precisamos criar o desencriptador do token de segurança

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Agora com o TokenHandler podemos criar o token

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }

    }
}