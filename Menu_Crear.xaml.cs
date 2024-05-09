using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class Menu_Crear : ContentPage
    {
        private readonly ArmaService _armaService;
        private readonly TipoArmaService _tipoArmaService;

        public Menu_Crear()
        {
            InitializeComponent();

            // Establecer el color de fondo de la página
            BackgroundColor = Color.FromHex("#5a61bd");

            // Proporcionar la URL base de la API al crear una instancia de ArmaService
            _armaService = new ArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");

            // Cargar los tipos de arma al inicializar la página
            CargarTiposArma();
            CargarRarezas(); // Llama al método para cargar las rarezas en el picker
        }

        // Método para cargar los tipos de arma
        private async Task CargarTiposArma()
        {
            try
            {
                // Obtener los tipos de arma desde el servicio
                var tiposArma = await _tipoArmaService.GetTiposArmaAsync();

                // Asignar los tipos de arma al Picker
                tipoArmaPicker.ItemsSource = tiposArma;
                tipoArmaPicker.ItemDisplayBinding = new Binding("Nombre");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción al cargar los tipos de arma
                await DisplayAlert("Error", $"Ocurrió un error al cargar los tipos de arma: {ex.Message}", "Aceptar");
            }
        }

        // Método para cargar las rarezas
        private async Task CargarRarezas()
        {
            try
            {
                // Simula la carga de datos de rarezas desde un servicio
                var rarezas = new[]
                {
                    "Seleccionar Rareza",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5"
                };

                rarezaPicker.ItemsSource = rarezas;
                rarezaPicker.SelectedIndex = 0; // Selecciona "Seleccionar Rareza" por defecto
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar las rarezas: {ex.Message}", "Aceptar");
            }
        }

        // Método para guardar un arma
        private async void GuardarClicked(object sender, EventArgs e)
        {
            try
            {
                var nuevaArma = new Arma
                {
                    nombre = nombreEntry.Text,
                    descripcion = descripcionEntry.Text,
                    rareza = Convert.ToInt32(rarezaPicker.SelectedItem.ToString()), // Convertir a entero
                    atk = Convert.ToInt32(atkEntry.Text),
                    crit_Dmg = Convert.ToDouble(critDmgEntry.Text),
                    cantidad = Convert.ToInt32(cantidadEntry.Text),
                    Tipo_ArmaId = ObtenerIdTipoArma(tipoArmaPicker.SelectedItem.ToString())
                };

                // Imprimir la URL y los datos de la solicitud antes de enviarla
                Console.WriteLine("URL de la solicitud POST:");
                Console.WriteLine(_armaService.ApiUrl);
                Console.WriteLine("Datos de la solicitud POST:");
                Console.WriteLine(JsonSerializer.Serialize(nuevaArma));

                await GuardarArmaAsync(nuevaArma);

                // Limpiar el formulario después de crear el arma
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al guardar el arma: {ex.Message}", "Aceptar");
            }
        }


        // Método para obtener el ID del tipo de arma
        private string ObtenerIdTipoArma(string nombreTipo)
        {
            var tiposDeArma = new[]
            {
                new { Nombre = "Espadas Cortas", Id = "1" },
                new { Nombre = "Espadones", Id = "2" },
                new { Nombre = "Arcos", Id = "3" },
                new { Nombre = "Lanzas", Id = "4" },
                new { Nombre = "Grimorios", Id = "5" }
             };

            foreach (var tipo in tiposDeArma)
            {
                if (tipo.Nombre == nombreTipo)
                {
                    return tipo.Id;
                }
            }

            return null; // Manejar este caso según tu lógica
        }

        // Método para guardar un arma en la API
        private async Task GuardarArmaAsync(Arma nuevaArma)
        {
            try
            {
                // Serializar la nueva arma a formato JSON
                var jsonNuevaArma = JsonSerializer.Serialize(nuevaArma);

                // Crear el contenido del mensaje HTTP
                var contenido = new StringContent(jsonNuevaArma, System.Text.Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
                using (var response = await _armaService.PostArmaAsync(contenido))
                {
                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        // Mostrar un mensaje de éxito
                        await DisplayAlert("Éxito", "Se ha creado el arma correctamente.", "Aceptar");
                    }
                    else
                    {
                        // Mostrar un mensaje de error con la respuesta del servidor
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Error", $"Ocurrió un error al crear el arma. Código de estado: {response.StatusCode}. Mensaje: {errorMessage}", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y mostrar un mensaje de error
                await DisplayAlert("Error", $"Ocurrió un error al crear el arma: {ex.Message}", "Aceptar");
            }
        }

        // Método para limpiar el formulario después de crear el arma
        private void LimpiarFormulario()
        {
            nombreEntry.Text = string.Empty;
            descripcionEntry.Text = string.Empty;
            rarezaPicker.SelectedIndex = 0;
            atkEntry.Text = string.Empty;
            critDmgEntry.Text = string.Empty;
            cantidadEntry.Text = string.Empty;
            tipoArmaPicker.SelectedIndex = 0;
        }

    }
}
