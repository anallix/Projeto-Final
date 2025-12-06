using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReservaApi.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RestauranteContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy =>
        {
            policy.AllowAnyOrigin()  // Aceita conexão de qualquer lugar
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReservaApi", Version = "v1" });

    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (System.IO.File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try 
    {
        var db = scope.ServiceProvider.GetRequiredService<RestauranteContext>();
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar banco: {ex.Message}");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.Run();
