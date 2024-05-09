using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _httpClient;
        public string ApiUrl { get; }

        public UsuarioService(string apiUrl)
        {
            _httpClient = new HttpClient();
            ApiUrl = apiUrl;
        }

        public async Task<Usuario> BuscarUsuarioAsync(string username)
        {
            try
            {
                Console.WriteLine($"Buscando usuario: {username}");

                var url = $"{ApiUrl}/Usuario?user={username}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(content);

                    // Encuentra el primer usuario con el nombre de usuario proporcionado
                    var usuario = usuarios.FirstOrDefault(u => u.user == username);

                    if (usuario != null)
                    {
                        Console.WriteLine($"Usuario encontrado: {JsonSerializer.Serialize(usuario)}");
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró el usuario: {username}");
                    }

                    return usuario;
                }
                else
                {
                    Console.WriteLine($"No se encontró el usuario: {username}");
                    return null; // Usuario no encontrado
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar el usuario: {ex.Message}");
                throw; // Manejar el error adecuadamente según tu aplicación
            }
        }

        public async Task<bool> VerificarCredencialesAsync(string username, string password)
        {
            try
            {
                var usuario = await BuscarUsuarioAsync(username);

                if (usuario != null && usuario.password == password)
                {
                    return true; // Credenciales válidas
                }
                else
                {
                    return false; // Credenciales inválidas
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar las credenciales: {ex.Message}");
                throw; // Manejar el error adecuadamente según tu aplicación
            }
        }
    }

    public class Usuario
    {
        public string user { get; set; }
        public string password { get; set; }
        public string id { get; set; }
    }
}
