using System.Text;
using App;
using App.Services;
using App.Services.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// run migrations
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
db.Database.Migrate();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    // Add database
    services.AddDbContext<DataBaseContext>(o =>
    {
        o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    // Login
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IUsuarioService, UsuarioService>();

    // crypto
    services.AddScoped<ICryptoService, Argon2Service>();

    // Jwt
    var key = Encoding.ASCII.GetBytes(Settings.Secret);
    services
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}
