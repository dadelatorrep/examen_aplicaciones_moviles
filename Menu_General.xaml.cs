using System;
using Microsoft.Maui.Controls;

namespace MauiAppUTN001;

public partial class Menu_General : ContentPage
{
    public Menu_General()
    {
        InitializeComponent();
    }

    private async void OnTiposDeArmaTapped(object sender, EventArgs e)
    {
        // Navegar a la página Menu_Tipo_Arma
        await Navigation.PushAsync(new TipoArma_Menu());
    }

    private async void OnArmaTapped(object sender, EventArgs e)
    {
        // Navegar a la página Menu_Arma
        await Navigation.PushAsync(new Arma_Menu());
    }
}
