
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRFTLatam.OrdersData.Infrastructure;
using PRFTLatam.OrdersData.Services.IServices;
using PRFTLatam.OrdersData.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// https://stackoverflow.com/questions/56310174/asp-net-core-mvc-connecting-to-existing-database-using-sql-server-authenticatio#:~:text=In%20appsettings.json%20add%20the%20following%20%28with%20your%20appropriate,you%20configure%20the%20IServiceCollection%20services%20add%20the%20following
// builder.Services.AddDbContext<OrdersDataContext>(
//     options => {options.UseSqlServer(GetConnectionString("SQLServerConnectionStr"));}
// );

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

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
