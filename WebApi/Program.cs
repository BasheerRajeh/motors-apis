using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.Common;
using WebApi.Models;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.Services.Common;
using WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
var corsUrls = builder.Configuration.GetValue<string>("CorsUrls").Split(';').ToList();
builder.Services.AddCors();

builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

// Add services to the container.

builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
                       ValidAudience = builder.Configuration["Jwt:ValidAudience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                   };
               });

//builder.Services.AddSingleton<FirebaseNotifier>();
builder.Services.AddAuthorization();

//builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//builder.Services.AddAuthorizationHandlers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddScoped<LoggedInUser>();

builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<KeyGenerator>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<UploadService>();

builder.Services.AddBusinessServices();

builder.Services.AddHttpClient(AppConstants.ApiExchangeRateClientName, x =>
{
    x.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExchangeRateApi"));
    x.Timeout = TimeSpan.FromSeconds(300);
});

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    corsUrls.ForEach(x => options.WithOrigins(x));

    //options.WithOrigins("http://localhost:3000", "http://localhost:4000");

});

app.UseRouting();

//app.UseRewriter(new RewriteOptions().AddRewrite(@"^confirm-email/?$", "/confirm-email/index.html", skipRemainingRules: true));

//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(
//                    Path.Combine(Directory.GetCurrentDirectory(),"content")),
//    RequestPath = "/content",
//    EnableDefaultFiles = true
//});

app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () =>
{
    return "API is runnig...";
});
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapFallbackToFile("index.html");
//    endpoints.MapGet("/", context =>
//    {
//        return context.Response.WriteAsync("API is running...");
//    });
//});

app.MapFallbackToFile("index.html"); ;

app.Run();
