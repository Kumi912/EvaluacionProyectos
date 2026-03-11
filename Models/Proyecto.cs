using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EvaluacionProyectosApi.Models;

public class Proyecto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    public string Titulo { get; set; } = null!;

    [BsonElement("integrantes")]
    public List<string> Integrantes { get; set; } = new();

    [BsonElement("dueñoId")]
    public string DueñoId { get; set; } = null!;

    [BsonElement("categorias")]
    public List<string> Categorias { get; set; } = new();

    [BsonElement("fechaEntrega")]
    public DateTime FechaEntrega { get; set; } = DateTime.UtcNow;

    [BsonElement("resumenRapido")]
    public string ResumenRapido { get; set; } = null!;

    [BsonElement("problemaResuelve")]
    public string ProblemaResuelve { get; set; } = null!;

    // Estos dos son opcionales (tienen el signo de interrogación ?)
    [BsonElement("instruccionesEspeciales")]
    public string? InstruccionesEspeciales { get; set; }

    [BsonElement("loMasDificil")]
    public string? LoMasDificil { get; set; }

    [BsonElement("archivosNube")]
    public List<string> ArchivosNube { get; set; } = new();

    [BsonElement("repositorioGitHub")]
    public string? RepositorioGitHub { get; set; }

    [BsonElement("linksGenerales")]
    public List<string> LinksGenerales { get; set; } = new();

    [BsonElement("estado")]
    public string Estado { get; set; } = "Pendiente";
}