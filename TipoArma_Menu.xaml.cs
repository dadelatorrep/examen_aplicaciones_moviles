using Microsoft.Maui.Controls;
using MauiAppUTN001.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace MauiAppUTN001
{
    public partial class TipoArma_Menu : ContentPage
    {
        public ObservableCollection<MenuOption> MenuOptions { get; set; }
        public TipoArma_Menu()
        {
            InitializeComponent();

            // Inicialización de MenuOptions con tus opciones de menú
            MenuOptions = new ObservableCollection<MenuOption>
            {
                new MenuOption { IconPath = "Icons/book.svg", Title = "Inventario", Description = "Muestra una lista de los tipos de armas disponibles", OnClickCommand = new Command(OnInventarioClicked) },
                new MenuOption { IconPath = "Icons/swords.png", Title = "Crear Tipo de Arma", Description = "Permite crear un nuevo tipo de arma", OnClickCommand = new Command(OnCrearArmaClicked) },
                new MenuOption { IconPath = "Icons/edit.svg", Title = "Editar Tipo de Arma", Description = "Permite editar los tipos de armas existentes", OnClickCommand = new Command(OnEditarArmasClicked) },
                new MenuOption { IconPath = "Icons/delete.svg", Title = "Eliminar Tipo de Arma", Description = "Permite eliminar los tipos de armas del inventario", OnClickCommand = new Command(OnEliminarArmaClicked) }
            };
            BindingContext = this;
        }
        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Navegar a la página 
            Navigation.PushAsync(new MainPage());
        }

        private void OnMenuItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = e.CurrentSelection.FirstOrDefault() as MenuOption;
            selectedOption?.OnClickCommand.Execute(null);
        }

        // Evento para la opción "Inventario" del menú
        private void OnInventarioClicked(object obj)
        {

            // Navegar a la página Menu_Inventario
            Navigation.PushAsync(new TipoArma_Read());
        }

        // Lógica para la opción "Crear Arma"
        private void OnCrearArmaClicked(object obj)
        {
            // Hacer la navegación a la página Menu_Crear
            Navigation.PushAsync(new TipoArma_Create());
        }

        // Lógica para la opción "Editar Armas"
        private void OnEditarArmasClicked(object obj)
        {
            // Navegar a la página 
            Navigation.PushAsync(new TipoArma_Update());
        }

        // Lógica para la opción "Eliminar Arma"
        private void OnEliminarArmaClicked(object obj)
        {
            // Navegar a la página 
            Navigation.PushAsync(new TipoArma_Delete());
        }

        // Método para manejar el evento Tapped de las tarjetas
        private void OnCardTapped(object sender, EventArgs e)
        {
            var selectedOption = (sender as Frame)?.BindingContext as MenuOption;
            selectedOption?.OnClickCommand.Execute(null);
        }

    }
}