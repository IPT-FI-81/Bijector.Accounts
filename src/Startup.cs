using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using IdentityServer4;
using Bijector.Infrastructure.Discovery;
using Bijector.Infrastructure.Repositories;
using Bijector.Accounts.Repositories;
using Bijector.Accounts.Models;
using Bijector.Accounts.Services;
using Bijector.Infrastructure.Dispatchers;
using Bijector.Infrastructure.Handlers;
using Bijector.Accounts.Messages.Queries;
using Bijector.Accounts.Messages.Commands;
using Bijector.Accounts.Handlers.Queries;
using Bijector.Accounts.Handlers.Commands;

namespace Bijector.Accounts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsul(Configuration);

            services.AddMongoDb(Configuration);

            services.AddMongoDbRepository<Account>();

            services.AddIdentityServer(options =>
            {
                options.IssuerUri = Configuration.GetSection("ConsulOptions:ServiceAddress").Get<string>();
                options.UserInteraction.LoginUrl = "/Accounts/LoginView";                
            }).
                AddDeveloperSigningCredential().
                AddInMemoryApiResources(Configs.GetApiResources()).
                AddInMemoryClients(Configs.GetClients(Configuration)).
                AddInMemoryIdentityResources(Configs.GetIdentityResources()).                
                AddProfileService<AccountProfileService>().
                AddCorsPolicyService<CORSPolicyService>();

            services.AddAuthentication(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            services.AddTransient<IPasswordHasher<Account>, SHAPasswordHasher>();
            services.AddTransient<IAccountStore, AccountStore>();            

            services.AddLocalApiAuthentication();

            services.AddHandleDispatchers();
            services.AddTransient<IQueryHandler<RegisterRequest, Account>, RegisterRequestHandler>();
            services.AddTransient<ICommandHandler<AddLinkedService>,AddLinkedServiceHandler>();

            services.AddControllersWithViews();
            //services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseConsul(lifetime);            

            app.UseRouting();

            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseIdentityServer();

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}