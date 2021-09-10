using FileForwaderTelegramBot.Bot;
using FileForwaderTelegramBot.Database;
using FileForwaderTelegramBot.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace FileForwaderTelegramBot
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
            services.AddDbContext<BotDb>(o => o.UseSqlite(Configuration["ConnectionString"]), ServiceLifetime.Scoped);

            services.AddScoped<ChatContext>();

            services.AddSingleton<BotClientHost>();
            services.AddSingleton<EventBusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BotDb>();
                db.Database.Migrate();

                var bot = scope.ServiceProvider.GetRequiredService<BotClientHost>();
                bot.Start(new CancellationTokenSource()).ConfigureAwait(false).GetAwaiter().GetResult();

                scope.ServiceProvider.GetRequiredService<EventBusService>();
            }
        }
    }
}
