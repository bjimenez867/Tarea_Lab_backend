using Microsoft.EntityFrameworkCore;
using SistemaParqueos.AccesoDatos.Contexto;
using SistemaParqueos.AccesoDatos.Implementaciones;
using SistemaParqueos.Dominio.InterfacesAD;
using SistemaParqueos.LogicaNegocio.Implementaciones;
using SistemaParqueos.Dominio.InterfazLN;
using SistemaParqueos.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ---------- Registro de servicios ----------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string PoliticaCors = "FrontendPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(PoliticaCors, policy =>
    {
        policy.WithOrigins("http://localhost:8100", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ParqueosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ParqueosDB")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClienteLN, ClienteLN>();
builder.Services.AddScoped<ITipoVehiculoLN, TipoVehiculoLN>();
builder.Services.AddScoped<IVehiculoLN, VehiculoLN>();
builder.Services.AddScoped<IParqueoLN, ParqueoLN>();
builder.Services.AddScoped<IEspacioParqueoLN, EspacioParqueoLN>();
builder.Services.AddScoped<ITarifaLN, TarifaLN>();
builder.Services.AddScoped<IIngresoVehiculoLN, IngresoVehiculoLN>();
builder.Services.AddScoped<IFacturaLN, FacturaLN>();
builder.Services.AddScoped<IDashboardLN, DashboardLN>();

var app = builder.Build();

// ---------- Pipeline HTTP ----------

app.UseMiddleware<ManejoErroresMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(PoliticaCors);

app.MapControllers();

app.Run();