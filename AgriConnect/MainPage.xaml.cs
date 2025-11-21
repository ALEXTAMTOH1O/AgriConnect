namespace AgriConnect
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Simulation de connexion en dur
            var demoUser = new { Name = "Utilisateur Test", Email = "test@example.com" };

            // Naviguer vers la page de sélection de rôle
            await Shell.Current.GoToAsync("roleselection");
        }

        private async void OnGoogleClicked(object sender, EventArgs e)
        {
            // Pas d'auth réelle: afficher un message indiquant que ceci est une simulation
            await DisplayAlert("Google", "Connexion via Google (simulation). Aucune authentification réelle disponible.", "OK");
        }
    }
}
