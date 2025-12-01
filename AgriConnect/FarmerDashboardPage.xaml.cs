using System.Collections.ObjectModel;

namespace AgriConnect;

public partial class FarmerDashboardPage : ContentPage
{
    public ObservableCollection<Product> MyProducts { get; set; }

	public FarmerDashboardPage()
	{
		InitializeComponent();

        MyProducts = new ObservableCollection<Product>
        {
            new Product 
            { 
                Name = "Plantains Mûrs", 
                Price = "80 XAF/kg", 
                Image = "plantains.jpg",
                Quantity = "10 tonnes",
                Description = "Plantains mûrs de qualité supérieure."
            },
            new Product 
            { 
                Name = "Tomates de Champ", 
                Price = "250 XAF/kg", 
                Image = "tomatoes.jpg",
                Quantity = "20 tonnes",
                Description = "Tomates fraîches."
            }
        };

        BindingContext = this;
	}

    private async void OnAddProductClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddProductPage));
    }

    private async void OnChatClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChatListPage));
    }

    private async void OnOrdersClicked(object sender, EventArgs e)
    {
        // Navigate to Orders page (to be implemented or reuse CartPage for now)
        await DisplayAlert("Info", "Page Mes Commandes à venir", "OK");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
}
