using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApi_API.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    //option => option.ReturnHttpNotAcceptable = true 
    ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("Log/VillaLog.txt",rollingInterval:RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
