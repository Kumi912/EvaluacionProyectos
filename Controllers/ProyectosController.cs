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

    // GET: api/proyectos -> Para listar todos los proyectos (útil para la barra de búsqueda y filtros)
    [HttpGet]
    public async Task<List<Proyecto>> GetProyectos()
    {
        return await _mongoService.Proyectos.Find(_ => true).ToListAsync();
    }

    // POST: api/proyectos -> Para que el Dueño del Proyecto envíe uno nuevo
    [HttpPost]
    public async Task<IActionResult> CrearProyecto([FromBody] Proyecto nuevoProyecto)
    {
        await _mongoService.Proyectos.InsertOneAsync(nuevoProyecto);
        return Ok(new { mensaje = "Proyecto enviado correctamente", proyecto = nuevoProyecto });
    }
}