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
            await DisplayAlert("Rôle sélectionné", "Vous avez choisi: Agriculteur", "OK");
        }

        private async void OnAcheteurTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Rôle sélectionné", "Vous avez choisi: Acheteur", "OK");
        }
    }
}