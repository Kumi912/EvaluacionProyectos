using EvaluacionProyectosApi.Models;
using EvaluacionProyectosApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace EvaluacionProyectosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluacionesController : ControllerBase
{
    private readonly MongoDbService _mongoService;

    public EvaluacionesController(MongoDbService mongoService)
    {
        _mongoService = mongoService;
    }

    // 1. Crear una nueva evaluación (Cuando el juez le da a "Comenzar a Calificar")
    [HttpPost]
    public async Task<IActionResult> CrearEvaluacion([FromBody] Evaluacion nuevaEvaluacion)
    {
        await _mongoService.Evaluaciones.InsertOneAsync(nuevaEvaluacion);
        
        // Magia: Cambiamos el estado del proyecto automáticamente a "En Revisión"
        var update = Builders<Proyecto>.Update.Set(p => p.Estado, "En Revisión");
        await _mongoService.Proyectos.UpdateOneAsync(p => p.Id == nuevaEvaluacion.ProyectoId, update);

        return Ok(new { mensaje = "Evaluación iniciada", evaluacion = nuevaEvaluacion });
    }

    // 2. Autoguardado (Se llama cada pocos segundos desde el celular)
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarEvaluacion(string id, [FromBody] Evaluacion evaluacionActualizada)
    {
        evaluacionActualizada.Id = id; // Aseguramos que el ID no cambie
        var resultado = await _mongoService.Evaluaciones.ReplaceOneAsync(e => e.Id == id, evaluacionActualizada);
        
        if (resultado.MatchedCount == 0)
            return NotFound(new { mensaje = "Evaluación no encontrada." });

        return Ok(new { mensaje = "Progreso guardado localmente" });
    }

    // 3. Obtener las evaluaciones de un juez (Para sus filtros y pestañas)
    [HttpGet("evaluador/{evaluadorId}")]
    public async Task<List<Evaluacion>> GetEvaluacionesPorMaestro(string evaluadorId)
    {
        return await _mongoService.Evaluaciones.Find(e => e.EvaluadorId == evaluadorId).ToListAsync();
    }

    // 4. Calcular el Promedio Global y listar jueces (Para la vista del Alumno)
    [HttpGet("proyecto/{proyectoId}/resultados")]
    public async Task<IActionResult> GetResultadosProyecto(string proyectoId)
    {
        var evaluaciones = await _mongoService.Evaluaciones.Find(e => e.ProyectoId == proyectoId).ToListAsync();

        if (!evaluaciones.Any())
            return Ok(new { mensaje = "Este proyecto aún no tiene calificaciones.", promedioGlobal = 0, evaluaciones = evaluaciones });

        // Cálculo matemático automático del promedio
        double promedioGlobal = evaluaciones.Average(e => e.CalificacionFinal);

        return Ok(new 
        { 
            promedioGlobal = Math.Round(promedioGlobal, 2), 
            totalJueces = evaluaciones.Count,
            detallesJueces = evaluaciones 
        });
    }
}