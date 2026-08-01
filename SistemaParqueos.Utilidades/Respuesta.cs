namespace SistemaParqueos.Utilidades;

public class Respuesta<T>
{
    public bool Exito { get; set; }
    public string? Mensaje { get; set; }
    public T? Datos { get; set; }

    public static Respuesta<T> Ok(T datos, string? mensaje = null) => new()
    {
        Exito = true,
        Mensaje = mensaje,
        Datos = datos
    };

    public static Respuesta<T> Error(string mensaje) => new()
    {
        Exito = false,
        Mensaje = mensaje,
        Datos = default
    };
}