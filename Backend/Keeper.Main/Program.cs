using Keeper.Common.Enums;
using Keeper.Common.Response;
using Keeper.Context.Config;
using Keeper.Main.Middleware;
using Keeper.Repos.Config;
using Keeper.Services.Config;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services
    .RegisterCORS()
    .RegisterDbContext(builder.Configuration)
    .RegisterRepos()
    .RegisterServices()
    .RegisterServices()
    .AddSwagger()
    .ConfigureApiBehavior()
    .ConfigAuth(builder.Configuration)
    .AddMailServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelExpandDepth(-1);
    });
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("Allow All");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
