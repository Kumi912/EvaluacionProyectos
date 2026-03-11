using MongoDB.Driver;
using EvaluacionProyectosApi.Models;

namespace EvaluacionProyectosApi.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
        // Leer la URL y el nombre de la base de datos desde appsettings.json
        var connectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
        var databaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Aquí le decimos a MongoDB que cree las colecciones si no existen
    public IMongoCollection<Proyecto> Proyectos => _database.GetCollection<Proyecto>("Proyectos");
    public IMongoCollection<Evaluacion> Evaluaciones => _database.GetCollection<Evaluacion>("Evaluaciones");
}