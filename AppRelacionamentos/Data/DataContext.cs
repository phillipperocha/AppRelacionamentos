using AppRelacionamentos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppRelacionamentos.Data
{
    // Não precisamos mais adicionar o DbSet<Users> já que iremos herdar o banco agora de IdentityDbContext
    // que nos dá suporte as classes do Entity Framework para Identity.

    // Se quiséssemos que nossas PKs fossem <ints> teriamos que colcoar a herança assim:
    // DataContext : IdentityDbContext<User, Role, int, IndentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    public class DataContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Como estamos usando o Identity agora precisamos chamar o Base aqui dentro do OnModelCreating()
            // Então aqui configura o schema que é preciso para o Entity Framework fazer o Identity.
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                // Esse espaço é para que o nosso Entity Framework saiba sobre o relacionamento entre
                // os nossos Users, Roles e nossos User Roles.
                // Isso faz um relacionamento many to many
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur=> ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                
            });
        }
    }
}
