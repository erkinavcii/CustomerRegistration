using Loggma1.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using static Loggma1.Controllers.ClientsController;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer= "your_issuer_here",
            ValidAudience="your_issuer_here",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD")),
            ValidateIssuer = false,
            ValidateAudience= false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();

// Add DbContext and database connection
builder.Services.AddDbContext<Loggma1.MyDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers(); // API Controller servisi
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // API isteklerini yönlendirir
    endpoints.MapRazorPages(); // Razor Pages isteklerini yönlendirir
});


app.Run();




