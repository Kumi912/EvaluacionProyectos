using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EvaluacionProyectosApi.Models;

public class Usuario : IValidatableObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Nombres")]
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
    public string Nombres { get; set; } = null!;

    [BsonElement("ApellidoPaterno")]
    public string? ApellidoPaterno { get; set; } // El "?" significa que puede venir vacío de inicio

    [BsonElement("ApellidoMaterno")]
    public string? ApellidoMaterno { get; set; } // El "?" significa que puede venir vacío de inicio

    [BsonElement("Correo")]
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
    public string Correo { get; set; } = null!;

    [BsonElement("Password")]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,}$", 
        ErrorMessage = "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial")]
    public string Password { get; set; } = null!;

    [BsonElement("Rol")]
    [Required(ErrorMessage = "El rol es obligatorio")]
    [RegularExpression("^(evaluador|participante)$", ErrorMessage = "Rol inválido")]
    public string Rol { get; set; } = null!;

    // Aquí está tu lógica experta: Exigir al menos un apellido
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(ApellidoPaterno) && string.IsNullOrWhiteSpace(ApellidoMaterno))
        {
            yield return new ValidationResult(
                "Debes ingresar al menos un apellido (paterno o materno).",
                new[] { nameof(ApellidoPaterno), nameof(ApellidoMaterno) });
        }
    }
}