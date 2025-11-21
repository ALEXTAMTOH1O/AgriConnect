using AgriConnect.Services;
using System.Collections.ObjectModel;

namespace AgriConnect;

public partial class CartPage : ContentPage
{
    public ObservableCollection<Product> CartItems => CartService.Instance.CartItems;

    public string TotalText => $"{CartService.Instance.GetTotal()} FCFA";

	public CartPage()
	{
		InitializeComponent();
        BindingContext = this;
        
        // Refresh total when items change (simple implementation)
        CartService.Instance.CartItems.CollectionChanged += (s, e) => 
        {
            OnPropertyChanged(nameof(TotalText));
        };
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is Product product)
        {
            CartService.Instance.RemoveFromCart(product);
        }
    }

    private async void OnCheckoutClicked(object sender, EventArgs e)
    {
        if (CartService.Instance.CartItems.Count == 0)
        {
            await DisplayAlert("Panier vide", "Veuillez ajouter des produits avant de commander.", "OK");
            return;
        }

        await DisplayAlert("Succès", "Votre commande a été enregistrée !", "OK");
        CartService.Instance.ClearCart();
        await Shell.Current.GoToAsync("..");
    }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(ProductsPage)}");
    }

    private async void OnChatClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChatListPage));
    }
}
