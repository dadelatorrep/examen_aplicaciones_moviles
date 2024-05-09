using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001.Services
{
    public class ArmaService
    {
        private readonly HttpClient _httpClient;
        public  string ApiUrl { get; }

        public ArmaService(string apiUrl)
        {
            _httpClient = new HttpClient();
            ApiUrl = apiUrl;
        }

        public async Task<HttpResponseMessage> PostArmaAsync(HttpContent content)
        {
            var url = $"{ApiUrl}/Arma";
            return await _httpClient.PostAsync(url, content);
        }


        public async Task<HttpResponseMessage> PutArmaAsync(string id, HttpContent content)
        {
            var url = $"{ApiUrl}/Arma/{id}";
            return await _httpClient.PutAsync(url, content);
           
        }

        public async Task<List<Arma>> BuscarArmasPorNombreAsync(string nombre)
        {
            var url = $"{ApiUrl}/Arma?nombre={nombre}";
            var response = await _httpClient.GetAsync(url);
            return await DeserializeResponse<List<Arma>>(response);
        }

        public async Task<Arma> BuscarArmaPorIdAsync(string id)
        {
            var url = $"{ApiUrl}/Arma/{id}";
            var response = await _httpClient.GetAsync(url);
            return await DeserializeResponse<Arma>(response);
        }

        public async Task<HttpResponseMessage> EliminarArmaAsync(string tipoArmaId, string armaId)
        {
            var url = $"{ApiUrl}/Tipo_Arma/{tipoArmaId}/Arma/{armaId}";
            return await _httpClient.DeleteAsync(url);
        }

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

        public async Task<List<Arma>> GetArmasAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/Arma");
            return await DeserializeResponse<List<Arma>>(response);
        }
    }

    public class Arma
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int rareza { get; set; }
        public int atk { get; set; }
        public double crit_Dmg { get; set; }
        public int cantidad { get; set; }
        public string id { get; set; }
        public string Tipo_ArmaId { get; set; }
    }
}
