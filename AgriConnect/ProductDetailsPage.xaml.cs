namespace AgriConnect;

[QueryProperty(nameof(SelectedProduct), "Product")]
public partial class ProductDetailsPage : ContentPage
{
    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
        }
    }

    public int CartItemCount => Services.CartService.Instance.ItemCount;

	public ProductDetailsPage()
	{
		InitializeComponent();
        BindingContext = this;

        Services.CartService.Instance.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Services.CartService.ItemCount))
            {
                OnPropertyChanged(nameof(CartItemCount));
            }
        };
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnContactSellerClicked(object sender, EventArgs e)
    {
        if (SelectedProduct != null)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "SellerName", SelectedProduct.SellerName }
            };
            await Shell.Current.GoToAsync(nameof(ChatPage), navigationParameter);
        }
    }

    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        if (SelectedProduct != null)
        {
            Services.CartService.Instance.AddToCart(SelectedProduct);
            await DisplayAlert("Succès", $"{SelectedProduct.Name} ajouté au panier !", "OK");
        }
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
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
