using System.Net;
using System.Text.Json;
using SistemaParqueos.Utilidades;

namespace SistemaParqueos.API.Middleware;

public class ManejoErroresMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManejoErroresMiddleware> _logger;

    public ManejoErroresMiddleware(RequestDelegate next, ILogger<ManejoErroresMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidacionNegocioException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio fallida");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var respuesta = Respuesta<object>.Error(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var respuesta = Respuesta<object>.Error("Ocurrió un error interno en el servidor.");
            await context.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
        }
    }
}