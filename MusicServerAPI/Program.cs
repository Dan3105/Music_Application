using Microsoft.EntityFrameworkCore;
using MusicServerAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using MusicServerAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MusicServerAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("https://localhost:5173", "http://localhost:5070", "https://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Add Jwt Auth
builder.Services.AddTransient<IJwtService, JwtService>();
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
       jwtOptions.RequireHttpsMetadata = false;
       jwtOptions.SaveToken = true;
       jwtOptions.TokenValidationParameters = tokenValidation;
       jwtOptions.Events = new JwtBearerEvents();
       jwtOptions.Events.OnMessageReceived = context =>
       {
           if(context.Request.Cookies.ContainsKey("access_token"))
           {
            context.Token = context.Request.Cookies["access_token"];
           }
           return Task.CompletedTask;
       };
   });
#endregion
// Add DbContext
builder.Services.AddDbContext<MusicServerAPIContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MusicServerAPI") ?? throw new InvalidOperationException("Connection string 'MusicServerAPI' not found.")));

builder.Services.ConfigureApplicationCookie(o => o.Cookie.Domain = "");
#region Add Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
#endregion



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors(myAllowSpecificOrigins);

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
