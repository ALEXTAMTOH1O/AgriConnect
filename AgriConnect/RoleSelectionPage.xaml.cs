using Microsoft.Maui.Controls;
using System;

namespace AgriConnect
{
    public partial class RoleSelectionPage : ContentPage
    {
        public RoleSelectionPage()
        {
            InitializeComponent();
        }

        private async void OnAgriculteurTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void OnAcheteurTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
