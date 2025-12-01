using Microsoft.Maui.Controls;
using System;
using Microsoft.Maui.Storage;

namespace AgriConnect
{
    public partial class RoleSelectionPage : ContentPage
    {
        public RoleSelectionPage()
        {
            InitializeComponent();
        }

        private async void OnBuyerClicked(object sender, EventArgs e)
        {
            Preferences.Set("UserRole", "Buyer");
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void OnFarmerClicked(object sender, EventArgs e)
        {
            Preferences.Set("UserRole", "Farmer");
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
