using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicService.Authorize;
using MusicService.Data;
using MusicService.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(o =>
{
    o.AddPolicy(name: myAllowSpecificOrigins,
        p =>
        {
            p.WithOrigins("https://locahost:5067")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.AddControllersWithViews().AddNewtonsoftJson(o =>
        o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//build Policy for service
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanCustomMusic", policy =>
    {
        policy.Requirements.Add(new RequireFollwingRoles("superadmin", "admin_can_custom_musics"));
    });
    options.AddPolicy("CanCustomArtist", policy =>
    {
        policy.Requirements.Add(new RequireFollwingRoles("superadmin", "admin_can_custom_artists"));
    });
    options.AddPolicy("CanCustomAny", policy =>
    {
        policy.Requirements.Add(new RequireFollwingRoles("superadmin"));
    });
});
builder.Services.AddSingleton<IAuthorizationHandler, RequireFollowingRolesHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Jwt handler
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
           if (context.Request.Cookies.ContainsKey("access_token"))
           {
               context.Token = context.Request.Cookies["access_token"];
           }
           return Task.CompletedTask;
       };
   });
#endregion

builder.Services.AddDbContext<MusicServiceContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MusicServer") ?? throw new InvalidOperationException("Connection string 'MusicServer' not found.")));

#region Add Repository
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseAuthorization();
app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);
app.MapControllers();

app.Run();
