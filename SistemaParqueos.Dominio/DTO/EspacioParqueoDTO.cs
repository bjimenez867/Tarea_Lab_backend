using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class EspacioParqueoDTO
{
    public int EspacioId { get; set; }

    [Required(ErrorMessage = "El parqueo es obligatorio")]
    public int ParqueoId { get; set; }

    [Required(ErrorMessage = "El número de espacio es obligatorio")]
    [MaxLength(20)]
    public string NumeroEspacio { get; set; } = null!;

    public bool Disponible { get; set; } = true;

    public bool Activo { get; set; } = true;
}