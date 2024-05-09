using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001.Services
{
    public class TipoArmaService
    {
        protected readonly HttpClient _httpClient;
        protected string ApiUrl { get; }

        public TipoArmaService(string apiUrl)
        {
            _httpClient = new HttpClient();
            ApiUrl = apiUrl;
        }

        // Método para enviar un nuevo tipo de arma a la API
        public async Task<HttpResponseMessage> PostTipoArmaAsync(HttpContent content)
        {
            return await _httpClient.PostAsync(ApiUrl + "/Tipo_Arma", content);
        }

        // Método para actualizar un tipo de arma existente en la API
        public async Task<HttpResponseMessage> PutTipoArmaAsync(string id, HttpContent content)
        {
            var url = $"{ApiUrl}/Tipo_Arma/{id}";
            return await _httpClient.PutAsync(url, content);
        }

        // Método para buscar tipos de armas por nombre
        public async Task<List<TipoArma>> BuscarTiposArmaPorNombreAsync(string nombre)
        {
            var url = $"{ApiUrl}/Tipo_Arma?nombre={nombre}";
            var response = await _httpClient.GetAsync(url);
            return await DeserializeResponse<List<TipoArma>>(response);
        }

        // Método para buscar un tipo de arma por su ID
        public async Task<TipoArma> BuscarTipoArmaPorIdAsync(string id)
        {
            var url = $"{ApiUrl}/Tipo_Arma/{id}";
            var response = await _httpClient.GetAsync(url);
            return await DeserializeResponse<TipoArma>(response);
        }

        // Método para eliminar un tipo de arma existente en la API
        public async Task<HttpResponseMessage> EliminarTipoArmaAsync(string id)
        {
            var url = $"{ApiUrl}/Tipo_Arma/{id}";
            return await _httpClient.DeleteAsync(url);
        }

        // Método para procesar la respuesta de la API y deserializar los datos
        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            else
            {
                throw new HttpRequestException($"Error al obtener el objeto. Código de estado: {response.StatusCode}");
            }
        }

        // Método para obtener todos los tipos de armas de la API
        public async Task<List<TipoArma>> GetTiposArmaAsync()
        {
            var response = await _httpClient.GetAsync(ApiUrl + "/Tipo_Arma");
            return await DeserializeResponse<List<TipoArma>>(response);
        }
    }

    public class TipoArma
    {
        public string Nombre { get; set; }
        public string Id { get; set; }
    }
}
