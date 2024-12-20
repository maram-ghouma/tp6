using tp6.Data;
using Microsoft.EntityFrameworkCore;
using tp6.Repositories.CategorieRepository;
using Microsoft.AspNetCore.Identity;
using tp6.Models;
using tp6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Ajouter RoleManager dans le DI container
builder.Services.AddScoped<IUserService, UserService>();

// configure strongly typed settings objects
var jwtSection = builder.Configuration.GetSection("JWTBearerTokenSettings");
builder.Services.Configure<JWTBearerTokenSettings>(jwtSection);
var jwtBearerTokenSettings = jwtSection.Get<JWTBearerTokenSettings>();
var key = System.Text.Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new
    TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtBearerTokenSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtBearerTokenSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

