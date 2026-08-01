using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class FacturaDTO
{
    public int FacturaId { get; set; }

    [Required(ErrorMessage = "El ingreso es obligatorio")]
    public int IngresoId { get; set; }

    public DateTime FechaFactura { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Las horas cobradas no pueden ser negativas")]
    public decimal HorasCobradas { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El monto total no puede ser negativo")]
    public decimal MontoTotal { get; set; }
}