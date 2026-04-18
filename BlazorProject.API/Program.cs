
using BlazorProject.API.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=devboard.db"));

            // 1. Adiciona a política de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BlazorClient", policy =>
                {
                    policy.WithOrigins("http://localhost:5199") // porta do seu Client
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // 2. Usa a política — ORDEM IMPORTA, tem que vir antes do UseAuthorization
            app.UseCors("BlazorClient");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
