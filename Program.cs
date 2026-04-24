using EvaluacionProyectosApi.Services;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
// 🚨 Le decimos al servidor Kestrel que permita recibir hasta 500 MB
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 524288000;
});

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddControllers();

// 🚨 Aumentar el límite de subida de formularios a 500 MB
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500 MB en bytes
});

// Añadir el servicio de Cloudinary
builder.Services.AddScoped<CloudinaryService>();

// El nuevo estándar para documentar en .NET 9
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // La nueva interfaz gráfica moderna
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();