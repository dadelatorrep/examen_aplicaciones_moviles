using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class TipoArma_Delete : ContentPage
    {
        private readonly TipoArmaService _tipoArmaService;

        public TipoArma_Delete()
        {
            InitializeComponent();
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        }

        private async void BuscarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text.Trim();

                if (!string.IsNullOrEmpty(id))
                {
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
                else
                {
                    await DisplayAlert("Error", "Debe especificar un ID para buscar.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el tipo de arma: {ex.Message}", "Aceptar");
            }
        }

        private async void EliminarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text.Trim();

                bool confirmarEliminacion = await DisplayAlert("Confirmar", "¿Estás seguro de que quieres eliminar este tipo de arma?", "Sí", "No");

                if (confirmarEliminacion)
                {
                    var response = await _tipoArmaService.EliminarTipoArmaAsync(id);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Se eliminó el tipo de arma correctamente.", "Aceptar");
                        LimpiarFormulario();
                    }
                    else
                    {
                        await DisplayAlert("Error", $"Ocurrió un error al eliminar el tipo de arma. Código de estado: {response.StatusCode}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al eliminar el tipo de arma: {ex.Message}", "Aceptar");
            }
        }

        private void MostrarDatosTipoArma(TipoArma tipoArma)
        {
            nombreEntry.Text = tipoArma.Nombre;

            nombreEntry.IsEnabled = true;
            idEntry.IsEnabled = false;
            nombreEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;

            // Habilitar el botón de eliminación una vez que se haya encontrado el tipo de arma
            ((Button)FindByName("Eliminar")).IsEnabled = true;
        }

        private void LimpiarFormulario()
        {
            idEntry.Text = string.Empty;
            nombreEntry.Text = string.Empty;

            idEntry.IsEnabled = true;
            nombreEntry.IsEnabled = false;
            ((Button)FindByName("Eliminar")).IsEnabled = false;
        }
    }
}
