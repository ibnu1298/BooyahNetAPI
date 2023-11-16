using BooyahNetAPI;
using BooyahNetAPI.DAL;
using BooyahNetAPI.Data.DAL;
using BooyahNetAPI.Helpers;
using BooyahNetAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Constring
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString(
//"BooyahConnection" //Development
"BooyahConnectionProd" //Production
)).EnableSensitiveDataLogging());

builder.Services.AddIdentity<User, CustomRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();

//Add Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Inject class DAL
builder.Services.AddScoped<IPayment, PaymentDAL>();
builder.Services.AddScoped<IPackage, PackageDAL>();
builder.Services.AddScoped<IUser, UserDAL>();
builder.Services.AddScoped<IAdmin, AdminDAL>();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("https://localhost:7771").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseMiddleware<JwtMiddleware>();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
