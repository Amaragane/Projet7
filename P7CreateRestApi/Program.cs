using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P7CreateRestApi.Middleware;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Services;
using P7CreateRestApi.Services.Auth;
using P7CreateRestApi.Services.Interfaces;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


// Configure JWT Settings
builder.Services.AddControllers();
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();



// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)), // Key est validée

        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer, // Issuer validé
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience, // Audience validée
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully");
            return Task.CompletedTask;
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "P7 Create Rest API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddIdentity<User, IdentityRole>() // ← Utilisez votre classe User ici
        .AddEntityFrameworkStores<LocalDbContext>()
        .AddDefaultTokenProviders();
builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401; // 401 au lieu de redirection
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403; // 403 au lieu de redirection
        return Task.CompletedTask;
    };
});
//builder.Services.AddAuthorization(options =>
//{
//    //options.DefaultPolicy = new AuthorizationPolicyBuilder()
//        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Force JWT
//        .RequireAuthenticatedUser()
//        .Build();
//});
// ============ REPOSITORIES ============
builder.Services.AddScoped<IBidListRepository, BidListRepository>();
builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ============ SERVICES ============
// Services d'authentification (nouveaux)
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Services métier existants
builder.Services.AddScoped<IBidListService, BidListService>();
builder.Services.AddScoped<ICurvePointService, CurvePointService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IRuleNameService, RuleNameService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseExceptionHandler("/error");
//app.UseMiddleware<TokenRenewalMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//app.MapGet("/error", (HttpContext context) =>
//    Results.Problem(detail: context.Features.Get<IExceptionHandlerFeature>()?.Error.Message));
app.Run();
