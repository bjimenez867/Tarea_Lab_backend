namespace SistemaParqueos.Utilitarios;

public class ValidacionNegocioException : Exception
{
    public ValidacionNegocioException(string mensaje) : base(mensaje)
    {
    }
}