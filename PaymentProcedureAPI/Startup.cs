using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NetCore.AutoRegisterDi;
using PaymentProcedureCore.IService;
using PaymentProcedureData.DatabaseContext;
using PaymentProcedureData.IRepository;
using System;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PaymentProcedureAPI
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
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("PaymentCloud"), sqlServerOptions => sqlServerOptions.CommandTimeout(600)));
            services.AddResponseCompression();
            services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                 (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddMemoryCache();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDirectoryBrowser();
            var serviceAssembly = Assembly.GetAssembly(typeof(IPaymentService));
            var repositoryAssembly = Assembly.GetAssembly(typeof(IPaymentRepository));
            services.RegisterAssemblyPublicNonGenericClasses(serviceAssembly, repositoryAssembly)
                    .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Repository"))
                    .AsPublicImplementedInterfaces();

            services.AddTransient<IAppDbContext, AppDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CORSPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
