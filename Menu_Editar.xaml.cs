using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class Menu_Editar : ContentPage
    {
        private readonly ArmaService _armaService;
        private readonly TipoArmaService _tipoArmaService;

        public Menu_Editar()
        {
            InitializeComponent();
            _armaService = new ArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarTiposArma();
        }

        private async Task CargarTiposArma()
        {
            try
            {
                var tiposArma = await _tipoArmaService.GetTiposArmaAsync();
                tipoArmaPicker.ItemsSource = tiposArma;
                tipoArmaPicker.ItemDisplayBinding = new Binding("Nombre");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar los tipos de arma: {ex.Message}", "Aceptar");
            }
        }

        private async void TipoArmaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                var tipoArmaSeleccionado = (TipoArma)picker.SelectedItem;
                ((Arma)BindingContext).Tipo_ArmaId = tipoArmaSeleccionado?.Id;
            }
            else
            {
                ((Arma)BindingContext).Tipo_ArmaId = null;
            }
        }

        private async void BuscarClicked(object sender, EventArgs e)
        {
            try
            {
                string nombre = nombreEntry.Text.Trim();

                if (string.IsNullOrEmpty(nombre))
                {
                    await DisplayAlert("Error", "Debe especificar al menos un nombre para buscar.", "Aceptar");
                    return;
                }

                var armas = await _armaService.BuscarArmasPorNombreAsync(nombre);

                if (armas != null && armas.Count > 0)
                {
                    MostrarDatosArma(armas[0]);
                    nombreEntry.Text = armas[0].nombre;
                    nombreEntry.Unfocus();
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró ningún arma con el nombre especificado.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al buscar el arma: {ex.Message}", "Aceptar");
            }
        }

        private async void MostrarDatosArma(Arma arma)
        {
            idEntry.Text = arma.id;
            descripcionEntry.Text = arma.descripcion;
            rarezaPicker.SelectedIndex = arma.rareza;
            atkEntry.Text = arma.atk.ToString();
            critDmgEntry.Text = arma.crit_Dmg.ToString();
            cantidadEntry.Text = arma.cantidad.ToString();

            string nombreTipoArma = await ObtenerNombreTipoArma(arma.Tipo_ArmaId);

            int index = tipoArmaPicker.Items.IndexOf(nombreTipoArma);
            if (index != -1)
            {
                tipoArmaPicker.SelectedIndex = index;
            }

            idEntry.IsEnabled = false;
            idEntry.TextColor = Colors.Black;
        }

        private async Task<string> ObtenerNombreTipoArma(string tipoArmaId)
        {
            try
            {
                var tipoArma = await _tipoArmaService.BuscarTipoArmaPorIdAsync(tipoArmaId);
                return tipoArma?.Nombre ?? "Tipo de Arma Desconocido";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el nombre del Tipo de Arma: {ex.Message}");
                return "Tipo de Arma Desconocido";
            }
        }

        private async void ActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                string id = idEntry.Text;
                var tipoArmaSeleccionado = (TipoArma)tipoArmaPicker.SelectedItem;
                string tipoArmaId = tipoArmaSeleccionado?.Id;

                if (rarezaPicker.SelectedIndex == 0)
                {
                    await DisplayAlert("Error", "Por favor selecciona una rareza.", "Aceptar");
                    return;
                }

                var arma = new Arma
                {
                    nombre = nombreEntry.Text,
                    descripcion = descripcionEntry.Text,
                    rareza = Convert.ToInt32(rarezaPicker.SelectedItem),
                    atk = Convert.ToInt32(atkEntry.Text),
                    crit_Dmg = Convert.ToDouble(critDmgEntry.Text),
                    cantidad = Convert.ToInt32(cantidadEntry.Text),
                    id = id,
                    Tipo_ArmaId = tipoArmaId
                };

                var jsonArma = JsonSerializer.Serialize(arma);
                var content = new StringContent(jsonArma, System.Text.Encoding.UTF8, "application/json");
                var response = await _armaService.PutArmaAsync(id, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Se actualizó el arma correctamente.", "Aceptar");
                    LimpiarFormulario();
                }
                else
                {
                    await DisplayAlert("Error", $"Ocurrió un error al actualizar el arma. Código de estado: {response.StatusCode}", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al actualizar el arma: {ex.Message}", "Aceptar");
            }
        }

        private void LimpiarFormulario()
        {
            idEntry.Text = string.Empty;
            nombreEntry.Text = string.Empty;
            descripcionEntry.Text = string.Empty;
            rarezaPicker.SelectedItem = null;
            atkEntry.Text = string.Empty;
            critDmgEntry.Text = string.Empty;
            cantidadEntry.Text = string.Empty;
            tipoArmaPicker.SelectedItem = null;
        }
    }
}
