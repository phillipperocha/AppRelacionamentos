using System.Threading.Tasks;
using AppRelacionamentos.Models;

namespace AppRelacionamentos.Data
{
    public interface IAuthRepository
    {
         
         // Primeiro vamos criar um método para registrar o usuário
         // Retornaremos um Task, porque será uma resposta assíncrona.
         Task<User> Register(User user, string password);

         // Segundo vamos fazer um método para Login
         Task<User> Login(string username, string password);

         // Terceiro vamos fazer o método para verificar se o usuário existe já no banco
         // Já que queremos que nossos usernames sejam únicos, e queremos prover a informação
         // Se o usuário já foi pego.

         Task<bool> UserExists(string username);
    }
}