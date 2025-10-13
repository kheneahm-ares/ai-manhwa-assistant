using Microsoft.EntityFrameworkCore; // transitive from npgsql
using FluentValidation;
using UserManagement.API.Services.Auth;
using Microsoft.AspNetCore.Identity;
using UserManagement.Data;
using UserManagement.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>((options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserManagement"));
});

// add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// registrations
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserValidator>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting(); // still call for CORS
app.UseCors();

app.UseAuthorization();

var api = app.MapGroup("/api");

var auth = api.MapGroup("/auth");
auth.MapAuthEndpoints();

app.Run();
