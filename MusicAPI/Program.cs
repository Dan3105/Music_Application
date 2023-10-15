using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicAPI.Data;
using MusicAPI.Repository;
using MusicAPI.Repository.Interface;
using MusicAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
    policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

#region JWT Validation Handler
//JWT Validation
builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    //Create Document
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT authentication", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description="This site is uses Bearer Token and you have to pass" +
        "it as Bearer<<space>>Token",
        Name= "Authorization",
        In=ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            },
            Scheme = "oauth2",
            Name="Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
        }
    });
});

var jwtKey = builder.Configuration.GetValue<string>("JwtSetting:Key");
var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

TokenValidationParameters tokenValidation = new TokenValidationParameters
{
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidateLifetime = true,
    ValidateAudience = false,
    ValidateIssuer = false,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton<TokenValidationParameters>(tokenValidation);
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(jwtOptions =>
   {
       jwtOptions.TokenValidationParameters = tokenValidation;
       jwtOptions.Events = new JwtBearerEvents();
       jwtOptions.Events.OnTokenValidated = async (context) =>
       {
           var ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
           var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
           var jwtToken =  context.SecurityToken as JwtSecurityToken;
           if (!await jwtService.IsValidated(jwtToken.RawData, ipAddress))
           {
               context.Fail("Invalid Token Details");
           }
       };
   });
#endregion

#region Injection Dependency
builder.Services.AddScoped<IUserRolesRepository, UserRolesRepository>();
#endregion

builder.Services.AddDbContext<MusicAPIContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MusicAPIContext") ?? throw new InvalidOperationException("Connection string 'MusicAPIContext' not found.")));

builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
