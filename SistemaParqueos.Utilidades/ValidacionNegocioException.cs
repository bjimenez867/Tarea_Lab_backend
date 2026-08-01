namespace SistemaParqueos.Utilidades;

public class ValidacionNegocioException : Exception
{
    public ValidacionNegocioException(string mensaje) : base(mensaje)
    {
    }
}