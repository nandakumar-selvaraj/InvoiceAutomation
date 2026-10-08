using InvoiceAutomationApi.Repositories;
using InvoiceAutomationApi.Repositories.Context;
using InvoiceAutomationApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Define a name for your policy
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// 2. Register the CORS services and configure options
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin() // Add allowed domains
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IInvoiceExtractionService, OpenAIInvoiceExtractionService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddDbContext<InvoiceAutomationDbContext>(options =>
    options.UseSqlite("Data Source=invoices.db"));

var app = builder.Build();

app.UseCors(myAllowSpecificOrigins);
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
