using HPSportsPlus.Models;
using HPSportsPlus.Services;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShopDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProductService, ProductService>();

//configure versioning
builder.Services.AddApiVersioning( options =>
{
    //Add report to responses
    options.ReportApiVersions = true;

    //Sets default version to 1.0
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

    //calls defualt version when api version is not explicitly specified.
    options.AssumeDefaultVersionWhenUnspecified = true;

    //sets versioning in header for http header versioning
    options.ApiVersionReader = new HeaderApiVersionReader("Product-Version");
}


);


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
