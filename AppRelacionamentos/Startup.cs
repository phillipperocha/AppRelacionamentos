using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRelacionamentos.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            // Adicionaremos o serviço, que será o contexto do Banco de dados
            // UseSqlite(Configuration.getConnectionString("SqliteConnection") para o SQLite
            // UseMySql(Configuration.GetConnectionString("MySqlConnection")) para o MySql
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Para adicionar o CORS, precisamos adicionar como serviço
            services.AddCors();   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Agora como adicionamos o serviço, precisamos usá-lo
            // Nós precisamos usá-lo com algumas configurações. Podemos dar um policyName ou uma ação
            // Usaremos uma ação, já que nao temos policy names definidas por enqunato.

            // Como é o começo do curso, deixaremos ele bem aberto e fraco. Permitindo tudo para podermos consumir.
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseMvc();
        }
    }
}
