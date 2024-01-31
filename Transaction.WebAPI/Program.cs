

using Transaction.WebAPI.Models;
using Transaction.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<PaymentInfoService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<PurchaseInfoService>();
builder.Services.AddScoped<StatementService>();

//fluent validation

builder.Services.AddScoped<PaymentValidator>();
builder.Services.AddScoped<PurchaseValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();


app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
