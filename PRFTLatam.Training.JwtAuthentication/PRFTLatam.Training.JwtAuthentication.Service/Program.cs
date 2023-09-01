using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PRFTLatam.Training.JwtAuthentication.Service.Enums;
using PRFTLatam.Training.JwtAuthentication.Service.Models;
using PRFTLatam.Training.JwtAuthentication.Service.Services;
using PRFTLatam.Training.JwtAuthentication.Service.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the JWT authentication middleware
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => // enable the JWT authenticating
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // server that created the token
        ValidateAudience = true, // receiver of the token is a valid recipient
        ValidateLifetime = true, // token has not expired
        ValidateIssuerSigningKey = true, // signing key is valid and is trusted by the server
        ValidIssuer = "https://localhost:5001",
        ValidAudience = "https://localhost:5001",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserService, UserService>();

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

app.Run();
