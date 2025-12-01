namespace AgriConnect;

public partial class AddProductPage : ContentPage
{
	public AddProductPage()
	{
		InitializeComponent();
	}

    private async void OnAddImageClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "Fonctionnalité d'ajout d'image à venir", "OK");
    }

    private async void OnPublishClicked(object sender, EventArgs e)
    {
        // Simulate publishing
        await DisplayAlert("Succès", "Votre annonce a été publiée avec succès !", "OK");
        await Shell.Current.GoToAsync(".."); // Go back
    }
}
