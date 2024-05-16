using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class TipoArma_Create : ContentPage
    {
        private readonly TipoArmaService _tipoArmaService;

        public TipoArma_Create()
        {
            InitializeComponent();
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        }

        // Método para guardar un nuevo tipo de arma
        private async void GuardarClicked(object sender, EventArgs e)
        {
            try
            {
                var nuevoTipoArma = new TipoArma
                {
                    Nombre = nombreEntry.Text
                };

                // Serializar el nuevo tipo de arma a formato JSON
                var jsonNuevoTipoArma = JsonSerializer.Serialize(nuevoTipoArma);

                // Crear el contenido del mensaje HTTP
                var contenido = new StringContent(jsonNuevoTipoArma, System.Text.Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
                using (var response = await _tipoArmaService.PostTipoArmaAsync(contenido))
                {
                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Mostrar un mensaje de éxito
                        await DisplayAlert("Éxito", "Se ha creado el tipo de arma correctamente.", "Aceptar");
                        // Limpiar el formulario después de crear el tipo de arma
                        LimpiarFormulario();
                    }
                    else
                    {
                        // Mostrar un mensaje de error con la respuesta del servidor
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", $"Ocurrió un error al crear el tipo de arma. Código de estado: {response.StatusCode}. Mensaje: {errorMessage}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y mostrar un mensaje de error
                await DisplayAlert("Error", $"Ocurrió un error al crear el tipo de arma: {ex.Message}", "Aceptar");
            }
        }

        // Método para limpiar el formulario después de crear el tipo de arma
        private void LimpiarFormulario()
        {
            nombreEntry.Text = string.Empty;
        }
    }
}
