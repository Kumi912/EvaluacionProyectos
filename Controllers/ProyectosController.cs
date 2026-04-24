using EvaluacionProyectosApi.Models;
using EvaluacionProyectosApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EvaluacionProyectosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProyectosController : ControllerBase
{
    private readonly MongoDbService _mongoService;

    public ProyectosController(MongoDbService mongoService)
    {
        _mongoService = mongoService;
    }

    // 🚨 NUEVO: GET: api/proyectos -> Trae TODOS los proyectos para el Evaluador
    [HttpGet]
    public async Task<List<Proyecto>> GetTodosLosProyectos()
    {
        // Va a MongoDB y trae absolutamente todos los proyectos registrados
        return await _mongoService.Proyectos.Find(_ => true).ToListAsync();
    }

    // GET: api/proyectos/{id} -> Trae un solo proyecto por su ID exacto
    [HttpGet("{id}")]
    public async Task<ActionResult<Proyecto>> GetProyectoPorId(string id)
    {
        var proyecto = await _mongoService.Proyectos.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (proyecto == null) return NotFound("Proyecto no encontrado");
        return Ok(proyecto);
    }

    // GET: api/proyectos/usuario/{dueñoId}
    [HttpGet("usuario/{dueñoId}")]
    public async Task<List<Proyecto>> GetProyectosPorUsuario(string dueñoId)
    {
        // Va a MongoDB y trae solo los proyectos que coincidan con el ID del dueño
        return await _mongoService.Proyectos.Find(p => p.DueñoId == dueñoId).ToListAsync();
    }

    // POST: api/proyectos -> Para que el Dueño del Proyecto envíe uno nuevo
    [HttpPost]
    public async Task<IActionResult> CrearProyecto([FromBody] Proyecto nuevoProyecto)
    {
        try
        {
            await _mongoService.CrearProyectoAsync(nuevoProyecto);
            return Ok(new { Mensaje = "Proyecto guardado con éxito" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al guardar en MongoDB: {ex.Message}");
        }
    }
}