using System.ComponentModel.DataAnnotations;

namespace EvaluacionProyectosApi.Models
{
    public class UsuarioLogin
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}