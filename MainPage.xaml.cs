using Microsoft.Maui.Controls;
using MauiAppUTN001.Services;
using System;

namespace MauiAppUTN001
{
    public partial class MainPage : ContentPage
    {
        private readonly UsuarioService _usuarioService;

        public MainPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService("https://6637fc174253a866a24c8af3.mockapi.io"); // Reemplaza la URL con la de tu API de usuario
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                // Verificar las credenciales usando UsuarioService
                bool credencialesValidas = await _usuarioService.VerificarCredencialesAsync(username, password);

                if (credencialesValidas)
                {
                    lblMensaje.Text = "Inicio de sesión exitoso";
                    await DisplayAlert("Éxito", "Inicio de sesión exitoso", "OK"); // Mensaje de confirmación
                    await Navigation.PushAsync(new Menu_General());
                    LimpiarCampos();
                }
                else
                {
                    lblMensaje.Text = "Nombre de usuario o contraseña inválidos";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
                await DisplayAlert("Error", "Se produjo un error al iniciar sesión", "OK");
            }
        }

        private void LimpiarCampos()
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            lblMensaje.Text = string.Empty;
        }
    }
}
