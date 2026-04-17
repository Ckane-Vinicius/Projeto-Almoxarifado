using ApiAlmoxarifado.Application;
using ApiAlmoxarifado.Application.Application_Interfaces;
using ApiAlmoxarifado.Data.Context;
using ApiAlmoxarifado.Data.Repository;
using ApiAlmoxarifado.Data.Repository_Interfaces;
using ApiAlmoxarifado.Domain.Services;
using ApiAlmoxarifado.Domain.Services_Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy =>
    {
        policy.WithOrigins("https://kind-meadow-095a1f110.1.azurestaticapps.net", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Application
builder.Services.AddScoped<IApplication, Application>();

//Domain
builder.Services.AddScoped<IApplicationServices, ApplicationServices>();

//Infra
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Front");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

