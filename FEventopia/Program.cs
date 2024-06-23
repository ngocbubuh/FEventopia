using FEventopia.DAO.DbContext;
using FEventopia.DAO.EntityModels;
using FEventopia.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FEventopia.Services.Settings;
using FEventopia.Controllers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Mail Services
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));

// Add VNPay Services
builder.Services.Configure<VnPaySetting>(builder.Configuration.GetSection("Vnpay"));

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

// Add Dependency Injection
builder.Services.AddWebAPIService();

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
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTION_STRING");
}

builder.Services.AddDbContext<FEventopiaDbContext>(options => { options.UseSqlServer(connection); });

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
            .WithExposedHeaders("JSON-Web-Token")
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseStatusCodePages();

app.UseExceptionHandler();

app.UseMiddleware<PerformanceMiddleware>();

app.UseHttpsRedirection();

app.UseCors("app-cors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
