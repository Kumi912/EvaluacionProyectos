using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EvaluacionProyectosApi.Models // (Cambia a Api.Models cuando lo pegues en la API)
{
    public class Evaluacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public string ProyectoId { get; set; } = "";
        public string EvaluadorId { get; set; } = "";
        public string EvaluadorNombre { get; set; } = "";
        
        public double CalificacionFinal { get; set; }
        public string ComentarioGeneral { get; set; } = "";
        
        public DateTime FechaEvaluacion { get; set; } = DateTime.Now;
    }
}









/* using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EvaluacionProyectosApi.Models;

public class Evaluacion
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("proyectoId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProyectoId { get; set; } = null!;

    [BsonElement("evaluadorId")]
    public string EvaluadorId { get; set; } = null!;

    [BsonElement("calificacionFinal")]
    public double CalificacionFinal { get; set; }

    [BsonElement("comentarioGeneral")]
    public string? ComentarioGeneral { get; set; }

    [BsonElement("detallesCriterios")]
    public List<CriterioEvaluacion> DetallesCriterios { get; set; } = new();

    [BsonElement("anotacionesEspecificas")]
    public List<AnotacionArchivo> AnotacionesEspecificas { get; set; } = new();
}

public class CriterioEvaluacion
{
    public string NombreCriterio { get; set; } = null!;
    public double Puntaje { get; set; }
    public string? Comentario { get; set; }
}

public class AnotacionArchivo
{
    public string ArchivoReferencia { get; set; } = null!;
    public double CoordenadaX { get; set; }
    public double CoordenadaY { get; set; }
    public string ComentarioAnotacion { get; set; } = null!;
} */