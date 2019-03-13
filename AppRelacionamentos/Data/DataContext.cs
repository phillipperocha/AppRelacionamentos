using AppRelacionamentos.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppRelacionamentos.Data
{
    public class DataContext : DbContext
    {

        // Para funcionar a herança com o DBContext nós precisamos ter acesso as opções do DBContext.
        // Sendo assim, precisamos instanciá-lo no construtor,linkando as opções da classe mãe

        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }

        // Agora na nossa classe DataContext, para dizer ao Entity Framework quais são as nossas entidades
        // Precisamos passar algumas propriedades.

        // Essas propriedades são do tipo DbSet
        // Precisamos dizer qual o tipo, passando ele no generics

        // A convenção é passar o nome no plural da nossa entidade, esse nome será o nome da
        // nossa tabela quando o banco de dados for criado.
        public DbSet<Value> Values { get; set; }

        // Isso é o que precisamos da nossa classe DataContext.
        // Agora precisamos informar a nossa aplicação sobre isso na nossa classe Startup.cs

    }
}
