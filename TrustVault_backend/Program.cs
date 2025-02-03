using Microsoft.EntityFrameworkCore;
using TrustVault_backend.DB_Context;
using TrustVault_backend.Repositories.Implementation;
using TrustVault_backend.Repositories.Interface;
using TrustVault_backend.Services.Implementation;
using TrustVault_backend.Services.Interface;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using TrustVault_backend.Repositories.Implementation.TrustVault.Repositories;
using TrustVault_backend.Helper;
using TrustVault_backend.Services;
using TrustVault_backend.SMTPSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 2)))); // Specify your MySQL version


builder.Services.AddSingleton<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IContactFormRepository, ContactFormRepository>();
builder.Services.AddScoped<IContactFormService, ContactFormService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddTransient<IDocumentSharingService, DocumentSharingService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IDocumentSharingRepository, DocumentSharingRepository>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
builder.Services.AddHostedService<OtpCleanupService>();


builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            //.WithOrigins("http://localhost:5173") // Replace with your frontend's URL
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/Home/Error");
}

// Use CORS
app.UseCors("AllowFrontend");
//Prepare Middleware for Authentication and Authorization
app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
