
using System.Collections.Concurrent;
using RealTimeChat.Hubs;
using RealTimeChat.Interface;
using RealTimeChat.Repository;

namespace RealTimeChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @".\realtimechat.json");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5173", "https://localhost:7079")
                                                                .AllowAnyHeader()
                                                                .AllowAnyMethod()
                                                                .AllowCredentials();
                                  });
            });
            builder.Services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });
            builder.Services.AddControllers();
            builder.Services.AddSingleton<ConcurrentDictionary<string, string>>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDirectMessageRepository, DirectMessageRepository>();
            builder.Services.AddScoped<IGroupChatRepository, GroupChatRepository>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddFirestoreDb("realtimechat - 8a931");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseHttpsRedirection();

            app.UseCors(myAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapHub<ChatHub>("/hub");
            app.MapControllers();

            app.Run();
        }
    }
}
