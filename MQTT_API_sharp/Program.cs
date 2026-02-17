using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using MQTT_API_sharp.Core.Interfaces;
using MQTT_API_sharp.DataWork;
using MQTT_API_sharp.DataWork.Repositories;

namespace MQTT_API_sharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.Name = "MqttApiSession"; // Имя вашей куки
					options.ExpireTimeSpan = TimeSpan.FromDays(1); // Время жизни сессии
					options.SlidingExpiration = true; // Продлевать сессию при активности
        
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Аналог SESSION_COOKIE_SECURE = True
					options.Cookie.SameSite = SameSiteMode.None; // Аналог SESSION_COOKIE_SAMESITE = 'None'
					
					// Важно для API: отключаем редирект на Login страницу при ошибке доступа
					options.Events.OnRedirectToLogin = context =>
					{
						context.Response.StatusCode = StatusCodes.Status401Unauthorized;
						return Task.CompletedTask;
					};
        
					// Важно для API: отключаем редирект при запрете доступа
					options.Events.OnRedirectToAccessDenied = context =>
					{
						context.Response.StatusCode = StatusCodes.Status403Forbidden;
						return Task.CompletedTask;
					};
				});

			builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
			// Чтение конфига repo
			var repoSection = builder.Configuration.GetSection("RepositorySettings");
			builder.Services.AddDbContextFactory<AppDbContext>(optionsAction =>
			{
				optionsAction.UseNpgsql(repoSection["ConnectionString"]); 
			});
			builder.Services.AddScoped<IDataRepository, DatabaseRepository>();

			var app = builder.Build();
            
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});
			
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
