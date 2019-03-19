using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRelacionamentos.Data;
using AppRelacionamentos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppRelacionamentos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));
            
            // *************************** INÍCIO IDENTITY *******************************

            // Existem muitas configurações dentro do Identity, iremos apenas usar algumas
            // Vamos usar o IdentityBuilder pra nos ajudar a fazer a configuração.
            // Se fosse uma aplicação MVC com Views pro razor, usríamos o services.AddIdentity
            // Como não queremos que nos redirecione a um lugar nem que guarde cookies, usaremos o Core
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                // Podemos listar aqui o que seu Password deve ter, como digitos em maiusculo, uppercase, etc
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            // Agora vamos fazer uma configuração que nos permite fazer uma query dos nossos usuários, e retornar
            // as suas roles ao mesmo tempo. Esse tipo de coisa é difícil de pesquisar porque a documentação da Microsoft
            // sobre isso não é muito boa ainda.

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>();
            // Ainda nos falta adicionar três novos serviços. Nós precisamos de um validador para os Roles
            // Precisamos de um Role Manager, para criar e remover roles
            // E precisamos de um método de sign in.
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            // *************************** FIM *******************************
            
            // Agora faremos para que todas as requisições ao sistema necessite estar autenticado
            services.AddMvc(Options => 
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    Options.Filters.Add(new AuthorizeFilter(policy));
                }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();   
            services.AddScoped<IAuthRepository, AuthRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMvc();
        }
    }
}
