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
            // Données en dur : exemple d'utilisateur
            var demoUser = new { Name = "Utilisateur Test", Email = "test@example.com" };

            await DisplayAlert("Connexion", $"Bienvenue, {demoUser.Name}!\nEmail: {demoUser.Email}", "OK");

            // Naviguer vers une autre page si nécessaire (non implémentée)
        }

        private async void OnGoogleClicked(object sender, EventArgs e)
        {
            // Pas d'auth réelle: afficher un message indiquant que ceci est une simulation
            await DisplayAlert("Google", "Connexion via Google (simulation). Aucune authentification réelle disponible.", "OK");
        }
    }
}
