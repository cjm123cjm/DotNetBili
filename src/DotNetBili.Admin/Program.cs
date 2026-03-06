using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNetBili.Admin.Extensions;
using DotNetBili.Common;
using DotNetBili.Common.Core;
using DotNetBili.Common.HttpContextUser;
using DotNetBili.Extension.ServicesExtension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule<AutofacModuleRegister>();
        builder.RegisterModule<AutofacPropertityModuleRegister>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        hostingContext.Configuration.ConfigureApplication();
    });

//ÊôÐÔ×¢Èë¿ØÖÆÆ÷
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

//automapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
AutoMapperConfig.RegisterMappings();

//ÅäÖÃ
builder.Services.AddSingleton(new AppSettings(builder.Configuration));
builder.Services.AddAllOptionRegister();

builder.ConfigureApplication();

//»º´æ
builder.Services.AddCacheSetup();

//ORM
builder.Services.AddSqlsugerSetup();

//jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUser, AspNetUser>();

var app = builder.Build();
app.ConfigureApplication();
app.UseApplicationSetup();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();