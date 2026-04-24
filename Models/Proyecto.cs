using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EvaluacionProyectosApi.Models;

// Creamos la misma sub-clase para MongoDB
public class IntegranteProyecto
{
    [BsonElement("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [BsonElement("rol")]
    public string Rol { get; set; } = string.Empty;
    
    [BsonElement("esLider")]
    public bool EsLider { get; set; } = false; // ¡NUEVO!
}

public class Proyecto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    public string Titulo { get; set; } = null!;

    // AHORA USA LA NUEVA CLASE DE INTEGRANTES
    [BsonElement("integrantes")]
    public List<IntegranteProyecto> Integrantes { get; set; } = new();

    [BsonElement("dueñoId")]
    public string DueñoId { get; set; } = null!;

    [BsonElement("categorias")]
    public List<string> Categorias { get; set; } = new();

    [BsonElement("fechaEntrega")]
    public DateTime FechaEntrega { get; set; } = DateTime.UtcNow;

    [BsonElement("resumenRapido")]
    public string ResumenRapido { get; set; } = null!;

    // Estos son opcionales (tienen el signo de interrogación ?)
    [BsonElement("problemaResuelve")]
    public string? ProblemaResuelve { get; set; }

    [BsonElement("instruccionesEspeciales")]
    public string? InstruccionesEspeciales { get; set; }

    [BsonElement("loMasDificil")]
    public string? LoMasDificil { get; set; }

    [BsonElement("logoUrl")]
    public string? LogoUrl { get; set; }

    [BsonElement("archivosNube")]
    public List<string> ArchivosNube { get; set; } = new();

    [BsonElement("repositorioGitHub")]
    public string? RepositorioGitHub { get; set; }

    [BsonElement("linksGenerales")]
    public List<string> LinksGenerales { get; set; } = new();

    [BsonElement("estado")]
    public string Estado { get; set; } = "Pendiente";
}