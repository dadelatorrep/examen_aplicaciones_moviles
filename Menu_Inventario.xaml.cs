using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppUTN001
{
public partial class Menu_Inventario : ContentPage
{
    private readonly ArmaService _armaService;

    public Menu_Inventario()
    {
        InitializeComponent();
        _armaService = new ArmaService("https://6633cce1f7d50bbd9b4ab447.mockapi.io");
        CargarDatos();
    }

    private async void CargarDatos()
    {
        try
        {
            var armas = await _armaService.GetArmasAsync();

            if (armas != null && armas.Count > 0)
            {
                var stackLayout = new StackLayout();

                foreach (var arma in armas)
                {
                    var armaStackLayout = CrearArmaStackLayout(arma);
                    stackLayout.Children.Add(CrearFrame(armaStackLayout));
                }

                Content = new ScrollView { Content = stackLayout };
            }
            else
            {
                Content = new Label
                {
                    Text = "No se encontraron armas",
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

    private StackLayout CrearArmaStackLayout(Arma arma)
    {
        string nombreTipoArma = ObtenerNombreTipoArma(arma.Tipo_ArmaId);

        var nombreLabel = new Label { Text = $"Nombre: {arma.nombre}", FontAttributes = FontAttributes.Bold, FontSize = 16 };
        var tipoArmaLabel = new Label { Text = $"Tipo de Arma: {nombreTipoArma}", FontSize = 16 };
        var cantidadLabel = new Label { Text = $"Cantidad: {arma.cantidad}", FontSize = 16 };

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += async (sender, e) => await MostrarDetalles(arma);
        var frameContent = new StackLayout
        {
            Children =
            {
                nombreLabel,
                tipoArmaLabel,
                cantidadLabel
            }
        };
        frameContent.GestureRecognizers.Add(tapGestureRecognizer);

        return frameContent;
    }

    private async Task MostrarDetalles(Arma arma)
    {
        await DisplayAlert("Detalles del Arma", $"Nombre: {arma.nombre}\nDescripción: {arma.descripcion}\nRareza: {arma.rareza}\nATK: {arma.atk}\nCrit. DMG: {arma.crit_Dmg}\nCantidad: {arma.cantidad}\nTipo de Arma: {ObtenerNombreTipoArma(arma.Tipo_ArmaId)}", "OK");
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
            WidthRequest = App.Current.MainPage.Width// Ajuste del ancho al 80% de la pantalla
        };
    }

    private string ObtenerNombreTipoArma(string idTipoArma)
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
            if (tipo.Id == idTipoArma)
            {
                return tipo.Nombre;
            }
        }

        return "Desconocido"; // Manejar este caso según tu lógica
    }
}

}
