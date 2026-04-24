using EvaluacionProyectosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionProyectosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivosController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public ArchivosController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("subir")]
        [DisableRequestSizeLimit] // 🚨 ESTA ETIQUETA DESACTIVA EL LÍMITE DE 28 MB
        public async Task<IActionResult> SubirArchivo([FromForm] IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("No se envió ningún archivo.");

            try
            {
                using var stream = archivo.OpenReadStream();
                
                // Usamos nuestro nuevo servicio de Cloudinary
                string url = await _cloudinaryService.SubirArchivoAsync(stream, archivo.FileName);

                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}