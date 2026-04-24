using Microsoft.AspNetCore.Mvc;
using EvaluacionProyectosApi.Models;
using EvaluacionProyectosApi.Services;
using BCrypt.Net;

namespace EvaluacionProyectosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly MongoDbService _mongoDbService;

    public UsuariosController(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] Usuario nuevoUsuario)
    {
        try
        {   
            Console.WriteLine($"\n[INFO] Intentando registrar al correo: {nuevoUsuario.Correo}...");

            var usuarioExistente = await _mongoDbService.GetUsuarioByCorreoAsync(nuevoUsuario.Correo);
            if (usuarioExistente != null)
            {
                Console.WriteLine("[ADVERTENCIA] El usuario ya existe en la base de datos.");
                return BadRequest("Ya existe un usuario registrado con este correo electrónico.");
            }

            // ---> AQUÍ SE APLICA EL CANDADO (HASH) <---
            // Reemplazamos la contraseña normal por su versión encriptada usando BCrypt
            nuevoUsuario.Password = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Password);

            await _mongoDbService.CreateUsuarioAsync(nuevoUsuario);
            Console.WriteLine("✅ [ÉXITO] Usuario guardado exitosamente en MongoDB.\n");

            return Ok(new { mensaje = "Usuario registrado exitosamente en MongoDB" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ [ERROR 500] Falló el registro en MongoDB.");
            Console.WriteLine($"Causa exacta: {ex.Message}\n");
            return StatusCode(500, new { error = "Error interno", detalle = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLogin credenciales)
    {
        try
        {
            Console.WriteLine($"\n[INFO] Intento de login para: {credenciales.Correo}");

            // 1. Buscamos al usuario en la base de datos por su correo
            var usuario = await _mongoDbService.GetUsuarioByCorreoAsync(credenciales.Correo);
            
            if (usuario == null)
            {
                // Si no existe, no le decimos "El correo no existe" por seguridad, damos un mensaje genérico.
                Console.WriteLine("[ADVERTENCIA] Correo no encontrado.");
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });
            }

            // 2. Comparamos la contraseña que escribió con el Hash guardado en MongoDB
            bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(credenciales.Password, usuario.Password);

            if (!passwordCorrecta)
            {
                Console.WriteLine("[ADVERTENCIA] Contraseña incorrecta.");
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });
            }

            // 3. ¡Éxito! Las credenciales son válidas.
            Console.WriteLine("✅ [ÉXITO] Login correcto.\n");
            
            // Le devolvemos un OK al celular, junto con los datos básicos del usuario
            return Ok(new 
            { 
                mensaje = "Login exitoso",
                id = usuario.Id,
                nombres = usuario.Nombres,
                rol = usuario.Rol
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ [ERROR 500] Falló el login.");
            Console.WriteLine($"Causa exacta: {ex.Message}\n");
            return StatusCode(500, new { error = "Error interno", detalle = ex.Message });
        }
    }
}