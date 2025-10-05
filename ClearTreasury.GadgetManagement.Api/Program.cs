using ClearTreasury.GadgetManagement.Api.Data;
using ClearTreasury.GadgetManagement.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("AppMainDb"))
        .UseAsyncSeeding(async (ctx, flag, ct) => await DbSeeding
            .SeedAsync(ctx, flag, builder.Environment.IsProduction(), ct))
        .UseSeeding((ctx, flag) => DbSeeding
            .SeedAsync(ctx, flag, builder.Environment.IsProduction(), CancellationToken.None)
            .GetAwaiter()
            .GetResult());
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

await using (var svcScope = app.Services.CreateAsyncScope())
{
    var dbContext = svcScope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
