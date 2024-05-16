using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
    public partial class TipoArma_Read : ContentPage
    {
        private readonly TipoArmaService _tipoArmaService;

        public TipoArma_Read()
        {
            InitializeComponent();
            _tipoArmaService = new TipoArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
            CargarDatos();
        }

        private async void CargarDatos()
        {
            try
            {
                var tiposDeArma = await _tipoArmaService.GetTiposArmaAsync();

                if (tiposDeArma != null && tiposDeArma.Count > 0)
                {
                    var stackLayout = new StackLayout();

                    foreach (var tipoArma in tiposDeArma)
                    {
                        var tipoArmaStackLayout = CrearTipoArmaStackLayout(tipoArma);
                        stackLayout.Children.Add(CrearFrame(tipoArmaStackLayout));
                    }

                    Content = new ScrollView { Content = stackLayout };
                }
                else
                {
                    Content = new Label
                    {
                        Text = "No se encontraron tipos de arma",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar los datos: {ex.Message}");
            }
        }

        private StackLayout CrearTipoArmaStackLayout(TipoArma tipoArma)
        {
            var nombreLabel = new Label { Text = $"Nombre: {tipoArma.Nombre}", FontAttributes = FontAttributes.Bold, FontSize = 16 };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) => await MostrarDetalles(tipoArma);
            var frameContent = new StackLayout
            {
                Children = { nombreLabel }
            };
            frameContent.GestureRecognizers.Add(tapGestureRecognizer);

            return frameContent;
        }

        private async Task MostrarDetalles(TipoArma tipoArma)
        {
            await DisplayAlert("Detalles del Tipo de Arma", $"Nombre: {tipoArma.Nombre}", "OK");
        }

        private Frame CrearFrame(StackLayout content)
        {
            return new Frame
            {
                Content = content,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                BackgroundColor = Color.FromHex("#9ca2ef"),
                CornerRadius = 10,
                HasShadow = true,
                WidthRequest = App.Current.MainPage.Width // Ajuste del ancho al 80% de la pantalla
            };
        }
    }
}
