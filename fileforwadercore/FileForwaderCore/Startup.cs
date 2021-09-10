using FileForwaderCore.AutoMapper;
using FileForwaderCore.Database;
using FileForwaderCore.Database.Entities;
using FileForwaderCore.Database.Repositories;
using FileForwaderCore.FileStorage;
using FileForwaderCore.MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace FileForwaderCore
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
            var connectionString = Configuration.GetSection("Db")["ConnectionString"];
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<FileForwaderDb>(options =>
                    options.UseNpgsql(connectionString)
                        .EnableSensitiveDataLogging(), ServiceLifetime.Scoped);

            services.AddScoped<IRepository<UserEntity>, BaseRepository<UserEntity>>();
            services.AddScoped<IRepository<MessageEntity>, MessageRepository>();
            services.AddScoped<IRepository<FileEntity>, FileRepository>();
            services.AddScoped<IRepository<StatusEventEntity>, StatusEventRepository>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddSingleton<IFileStorageService, FileStorageService>();
            services.AddScoped<IMessageBrokerService, MessageBrokerService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileForwaderCore", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileForwaderCore v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FileForwaderDb>();
                db.Database.Migrate();

                var test = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
            }
        }
    }
}
