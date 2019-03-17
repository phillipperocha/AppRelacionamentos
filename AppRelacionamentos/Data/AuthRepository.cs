using System;
using System.Threading.Tasks;
using AppRelacionamentos.Models;
using Microsoft.EntityFrameworkCore;

namespace AppRelacionamentos.Data
{
    // Primeiro estaremos usando a interface IAuthRepository
    public class AuthRepository : IAuthRepository
    {
        // 2º Injetar a dependência do DataContext no parâmetro, e inicializá-lo pelos parâmetros.
        // Agora temos acesso ao nosso contexto em nosso repositório.
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;

        }

        public async Task<User> Login(string username, string password)
        {
            // encontrar o usuário
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null) {
                return null;
            } 

            // Criamos um método pra verificar se o password é o correto
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
                return null;
            }

            return user;
        }

        // Vamos passar para o método o password, o hash e o salt.
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Para isso, faremos de maneira similar ao Password register, a diferença é que agora
            // passaremos no HMAC a chave pra ele testar.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // Agora ele vai comparar o computedHash, utilizando a chave do passwordSalt
                // E agora deve ser a mesma que colocamos no início
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Temos que verificar agora se todos os vetores do array são iguais um ao outro
                for (int i = 0; i < computedHash.Length; i++) 
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

            }
            
            return true;
        }

        // 3º Essemétodo vai pegar a nossa entidade User, e pegar a string password e vai registrar
        // esse usuário com essa senha. O que não queremos é guardar esse password como ele vem.
        // Precisaremos convertê-lo em um PasswordHash e depois num PasswordSalt.
        public async Task<User> Register(User user, string password)
        {
            // como estamos guardando eles como um array de bytes, declararemos assim:
            byte[] passwordHash, passwordSalt;
            // Queremos passar o passwordHash pro método como uma referência e não como um valor.
            // E nós fazemos isso passando uma chave-reservada chamada 'out'
            // Agora passaremos uma referência a essas variáveis, e quando elas forem atualizadas dentro 
            // do contexto do CreatePassword, elas também serão atualizadas fora
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // O 'out' faz com que o método de dois retornos nesse contexto e salve como variáveis
            // para que possamos utilizá-las.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Usaremos uma classe de segurança do sistema chamado Cryptography para fazer um hash.
            // Usaremos um using() para dizer que tudo que esta ali dentro estará disponível assim que acabarmos.
            // Assim poderemos guardar tanto o Hash quanto o Salt referente a esse uso e esse momento que encriptamos.
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await this.context.Users.AnyAsync(x => x.Username == username))
            return true;

            return false;
        }
    }
}