using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);
DependencyInjectionConfig.ConfigureServices(builder.Services, builder.Configuration);

var invariantCulture = CultureInfo.InvariantCulture;
var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(invariantCulture),
    SupportedCultures = new[] { invariantCulture },
    SupportedUICultures = new[] { invariantCulture }
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DefaultModelsExpandDepth(-1);
});

app.UseStaticFiles();
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () =>
{
    return Task.FromResult<IActionResult>(new OkResult());
});


#if DEBUG
app.MapControllers()
  .AllowAnonymous();
#else
 app.MapControllers();
#endif

app.Run();
