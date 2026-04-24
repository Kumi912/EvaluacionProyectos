using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace EvaluacionProyectosApi.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            // REEMPLAZA ESTOS 3 VALORES CON TUS DATOS DE CLOUDINARY
            Account account = new Account(
                "dykv8wmpn", // Cloud Name 
                "837654662547742", // Pega tu API Key
                "Cgz5mCESUKc3bMMD8qdBAgb3Wss" // Pega tu API Secret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> SubirArchivoAsync(Stream archivoStream, string nombreArchivo)
        {
            try
            {
                // AutoUploadParams es magia pura: detecta si es video, imagen, pdf o word y lo procesa solo
                var uploadParams = new AutoUploadParams()
                {
                    File = new FileDescription(nombreArchivo, archivoStream),
                    Folder = "evidencias_proyectos" // Se creará esta carpeta en tu nube
                };

                // 🚨 UploadLargeAsync es la magia: corta el video en pedacitos para que el internet no colapse
                var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);

                // Si todo sale bien, nos devuelve la URL segura (https)
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir a Cloudinary: {ex.Message}");
            }
        }
    }
}