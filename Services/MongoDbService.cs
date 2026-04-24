using MongoDB.Driver;
using MongoDB.Bson;
using EvaluacionProyectosApi.Models;

namespace EvaluacionProyectosApi.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Usuario> _usuariosCollection;
    private readonly IMongoCollection<Proyecto> _proyectosCollection;

    public MongoDbService(IConfiguration configuration)
    {
        try
        {
            var connectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDbSettings:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            // Hacer un "Ping" rápido para probar la conectividad
            bool isMongoLive = _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(2000);
            if (isMongoLive)
            {
                Console.WriteLine("\n✅ ÉXITO: Conexión a MongoDB Atlas establecida correctamente.\n");
            }

            _usuariosCollection = _database.GetCollection<Usuario>("Usuarios");
            _proyectosCollection = _database.GetCollection<Proyecto>("proyectos");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ ERROR CRÍTICO DE CONEXIÓN: No se pudo conectar a MongoDB.");
            Console.WriteLine($"Detalle del error: {ex.Message}\n");
        }
    }

    public IMongoCollection<Proyecto> Proyectos => _proyectosCollection;
    public IMongoCollection<Evaluacion> Evaluaciones => _database.GetCollection<Evaluacion>("Evaluaciones");

    public async Task CreateUsuarioAsync(Usuario nuevoUsuario)
    {
        await _usuariosCollection.InsertOneAsync(nuevoUsuario);
    }

    public async Task<Usuario?> GetUsuarioByCorreoAsync(string correo)
    {
        return await _usuariosCollection.Find(u => u.Correo == correo).FirstOrDefaultAsync();
    }

    public async Task CrearProyectoAsync(Proyecto proyecto)
    {
        await _proyectosCollection.InsertOneAsync(proyecto);
    }
}