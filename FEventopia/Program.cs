using FEventopia.Controllers.Middlewares;
using FEventopia.Repositories.DbContext;
using FEventopia.Repositories.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Repositories.Repositories;
using FEventopia.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FEventopia.Services.Services;
using FEventopia.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Mail Services
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(AutoMapperSetting).Assembly);

builder.Services.AddControllers(op => op.SuppressAsyncSuffixInActionNames = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FEventopia API", Version = "v.10.24" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token!",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add ConnectionStrings
var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    /// --------------WARNING------------------ ///
    ///   SELECT CONNECTIONSTRING CAREFULLY    ///
    // ---------------------------------------- //

    //If test in LocalDB
    //connection = builder.Configuration.GetConnectionString("LOCAL_CONNECTION_STRING");
    //If test in AzureDB
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");

    // ---------------------------------------- //
    ///   SELECT CONNECTIONSTRING CAREFULLY    ///
    /// --------------WARNING------------------ ///
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

builder.Services.AddDbContext<FEventopiaDbContext>(options => options.UseSqlServer(connection));

// Add Authentication
builder.Services.AddIdentity<Account, IdentityRole>()
        .AddEntityFrameworkStores<FEventopiaDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("app-cors",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination")
            .AllowAnyMethod();
        });
});

//Add Scope
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddSingleton<GlobalExceptionMiddleware>();
//builder.Services.AddSingleton<PerformanceMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//app.UseSwagger();
//app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseMiddleware<GlobalExceptionMiddleware>();

//app.UseMiddleware<PerformanceMiddleware>();

app.UseHttpsRedirection();

app.UseCors("app-cors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
