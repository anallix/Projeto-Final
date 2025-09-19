using Microsoft.EntityFrameworkCore;
using RestauranteAPI.Data; // <--- ADICIONE ESTA LINHA!

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();

// A configuração do DbContext já estava correta, mas precisava da linha 'using' acima.
builder.Services.AddDbContext<RestauranteContext>(
    options => options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
