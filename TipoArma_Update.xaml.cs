using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class TipoArma_Update : ContentPage
    {
        private readonly TipoArmaService _tipoArmaService;

        public TipoArma_Update()
        {
            InitializeComponent();
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        }

        private async void BuscarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text.Trim();

                if (string.IsNullOrEmpty(id))
                {
                    await DisplayAlert("Error", "Debe especificar un ID para buscar.", "Aceptar");
                    return;
                }

                var tipoArma = await _tipoArmaService.BuscarTipoArmaPorIdAsync(id);

                if (tipoArma != null)
                {
                    MostrarDatosTipoArma(tipoArma);
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró ningún tipo de arma con el ID especificado.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el tipo de arma: {ex.Message}", "Aceptar");
            }
        }

        private void MostrarDatosTipoArma(TipoArma tipoArma)
        {
            idEntry.Text = tipoArma.Id;
            nombreEntry.Text = tipoArma.Nombre;

            idEntry.IsEnabled = false;
            idEntry.TextColor = Colors.Black;
        }

        private async void ActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text;

                var tipoArma = new TipoArma
                {
                    Id = id,
                    Nombre = nombreEntry.Text
                };

                var jsonTipoArma = JsonSerializer.Serialize(tipoArma);
                var content = new StringContent(jsonTipoArma, System.Text.Encoding.UTF8, "application/json");
                var response = await _tipoArmaService.PutTipoArmaAsync(id, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Se actualizó el tipo de arma correctamente.", "Aceptar");
                    LimpiarFormulario();
                }
                else
                {
                    await DisplayAlert("Error", $"Ocurrió un error al actualizar el tipo de arma. Código de estado: {response.StatusCode}", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al actualizar el tipo de arma: {ex.Message}", "Aceptar");
            }
        }

        private void LimpiarFormulario()
        {
            idEntry.Text = string.Empty;
            nombreEntry.Text = string.Empty;
            idEntry.IsEnabled = true;
        }
    }
}
