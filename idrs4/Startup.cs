using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using idrs4.Data;
using idrs4.Models;
using idrs4.Services;
using idrs4.Configuration;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace idrs4
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
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(option => {
                option.Password.RequireUppercase = false;
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
            })
                //.AddRoles<IdentityRole>()
                
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #region 跨域
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:5001")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
            // 该方法是将所有用户Claim发送给客户端
            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                 //20180814 不再使用内存测试 启动数据库EF 迁移 持久化
                 .AddInMemoryPersistedGrants()
                 //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
                 //.AddInMemoryApiResources(Resources.GetApiResources())
                 //.AddInMemoryClients(Clients.Get())

                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                             sql => sql.MigrationsAssembly(migrationsAssembly));
                 })
                  .AddOperationalStore(options =>
                  {
                      options.ConfigureDbContext = builder =>
                          builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                              sql => sql.MigrationsAssembly(migrationsAssembly));

                      // this enables automatic token cleanup. this is optional.
                      options.EnableTokenCleanup = true;
                      options.TokenCleanupInterval = 30;
                  })



                //.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                //.AddAspNetIdentity<IdentityRole>()
                .AddJwtBearerClientAuthentication();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //初始化数据库 塞入数据库数据 新建 数据库 或者数据库文件丢失时使用！
            //InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        //初始化塞入数据库client 跟resources
        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
