using ClearTreasury.GadgetManagement.Api;
using ClearTreasury.GadgetManagement.Api.Data;
using ClearTreasury.GadgetManagement.Api.Extensions;
using ClearTreasury.GadgetManagement.Api.Infrastructure;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add services to the container.
builder.Services.AddBoundValidatedOptions<JwtAuthOptions>("JwtAuth");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AppMainDb"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        var jwtOptions = builder.Configuration.GetSection("JwtAuth").Get<JwtAuthOptions>()!;

        opts.TokenValidationParameters.ValidIssuer = jwtOptions.Issuer;
        opts.TokenValidationParameters.ValidAudience = jwtOptions.Audience;
        opts.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(jwtOptions.GetSecretKeyBytes());
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.CanManageGadgets, policy => policy.RequireRole(AppStaticRoles.Manager))
    .AddPolicy(AuthPolicies.CanViewGadgets, policy => policy.RequireRole(AppStaticRoles.User, AppStaticRoles.Manager));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    await DbSeeding.MigrateAndSeed(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
