using System.ComponentModel.DataAnnotations;

namespace SistemaParqueos.Dominio.DTO;

public class ClienteDTO
{
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [MaxLength(150)]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "La cédula es obligatoria")]
    [MaxLength(25)]
    public string Cedula { get; set; } = null!;

    [MaxLength(25)]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    [MaxLength(254)]
    public string? Correo { get; set; }

    public bool Activo { get; set; } = true;
}