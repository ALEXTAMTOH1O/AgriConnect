namespace AgriConnect;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private async void OnChangePhotoClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Fonctionnalité de changement de photo à venir", "OK");
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Succès", "Profil mis à jour avec succès !", "OK");
        await Shell.Current.GoToAsync("..");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Déconnexion", "Voulez-vous vraiment vous déconnecter ?", "Oui", "Non");
        if (confirm)
        {
            // Clear navigation stack and go to RoleSelection
            Application.Current.MainPage = new AppShell();
        }
    }
}
