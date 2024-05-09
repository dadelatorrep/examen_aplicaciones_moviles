using System.Windows.Input;

namespace MauiAppUTN001.Models
{
    public class MenuOption
    {
        public string IconPath { get; set; } // Propiedad para almacenar la ruta del archivo SVG
        public string Title { get; set; }
        public string Description { get; set; }
        public ICommand OnClickCommand { get; set; }
    }
}
