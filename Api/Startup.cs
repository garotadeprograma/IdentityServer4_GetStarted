using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Api
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
            services.AddControllers();

            //Add o serviço de autenticação na injeção de dependência
            //Configura o Bearer como esquema padrão
            services.AddAuthentication("Bearer")
                    //JWT = Json Web Token
                    .AddJwtBearer("Bearer", options =>
                                            {
                                                options.Authority = "https://localhost:5001";
                                                options.TokenValidationParameters = new TokenValidationParameters
                                                {
                                                    ValidateAudience = false
                                                };
                                            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Add o middleware ao pipeline do core para autenticação  seja executada automaticamente a cada chamada da API
            app.UseAuthentication();

            //Aciona o middleware de autorização pra garantir que o endpoint não seja acessado por anônimos
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
