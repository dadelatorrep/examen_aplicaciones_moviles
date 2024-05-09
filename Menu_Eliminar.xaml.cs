using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class Menu_Eliminar : ContentPage
    {
        private readonly ArmaService _armaService;

        public Menu_Eliminar()
        {
            InitializeComponent();
            _armaService = new ArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        }

        private async void BuscarClicked(object sender, EventArgs e)
        {
            try
            {
                string nombre = nombreEntry.Text.Trim();

                if (!string.IsNullOrEmpty(nombre))
                {
                    var armas = await _armaService.BuscarArmasPorNombreAsync(nombre);

                    if (armas != null && armas.Count > 0)
                    {
                        MostrarDatosArma(armas[0]);
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontró ningún arma con el nombre especificado.", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Debe especificar un nombre para buscar.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el arma: {ex.Message}", "Aceptar");
            }
        }

        private async void EliminarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text.Trim();

                bool confirmarEliminacion = await DisplayAlert("Confirmar", "¿Estás seguro de que quieres eliminar este arma?", "Sí", "No");

                if (confirmarEliminacion)
                {
                    var arma = await _armaService.BuscarArmaPorIdAsync(id);

                    if (arma != null && arma.Tipo_ArmaId != null)
                    {
                        string tipoArmaId = arma.Tipo_ArmaId;

                        var response = await _armaService.EliminarArmaAsync(tipoArmaId, id);

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Éxito", "Se eliminó el arma correctamente.", "Aceptar");
                            LimpiarFormulario();
                        }
                        else
                        {
                            await DisplayAlert("Error", $"Ocurrió un error al eliminar el arma. Código de estado: {response.StatusCode}", "Aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo obtener el tipo de arma asociado al arma.", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al eliminar el arma: {ex.Message}", "Aceptar");
            }
        }

        private void MostrarDatosArma(Arma arma)
        {
            idEntry.Text = arma.id;
            nombreEntry.Text = arma.nombre;
            descripcionEntry.Text = arma.descripcion;
            rarezaEntry.Text = arma.rareza.ToString();
            atkEntry.Text = arma.atk.ToString();
            critDmgEntry.Text = arma.crit_Dmg.ToString();
            cantidadEntry.Text = arma.cantidad.ToString();
            tipoArmaIdEntry.Text = arma.Tipo_ArmaId;

            idEntry.IsEnabled = false;
            nombreEntry.IsEnabled = false;
            descripcionEntry.IsEnabled = false;
            rarezaEntry.IsEnabled = false;
            atkEntry.IsEnabled = false;
            critDmgEntry.IsEnabled = false;
            cantidadEntry.IsEnabled = false;
            tipoArmaIdEntry.IsEnabled = false;

            idEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            nombreEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            descripcionEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            rarezaEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            atkEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            critDmgEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            cantidadEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            tipoArmaIdEntry.TextColor = Microsoft.Maui.Graphics.Colors.Black;
        }

        private void LimpiarFormulario()
        {
            idEntry.Text = string.Empty;
            nombreEntry.Text = string.Empty;
            descripcionEntry.Text = string.Empty;
            rarezaEntry.Text = string.Empty;
            atkEntry.Text = string.Empty;
            critDmgEntry.Text = string.Empty;
            cantidadEntry.Text = string.Empty;
            tipoArmaIdEntry.Text = string.Empty;
        }
    }
}
