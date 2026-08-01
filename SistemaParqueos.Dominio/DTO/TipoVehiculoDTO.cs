using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class TipoVehiculoDTO
{
    public int TipoVehiculoId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(100)]
    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; } = true;
}