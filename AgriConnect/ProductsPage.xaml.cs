using System.Collections.ObjectModel;

namespace AgriConnect;

public partial class ProductsPage : ContentPage
{
    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Product> FilteredProducts { get; set; }
    
    public int CartItemCount => Services.CartService.Instance.ItemCount;

	public ProductsPage()
	{
		InitializeComponent();

        Products = new ObservableCollection<Product>
        {
            new Product 
            { 
                Name = "Plantains Mûrs", 
                Price = "350F/KG", 
                Image = "plantains.jpg",
                Description = "Plantains mûrs de qualité supérieure, parfaits pour la friture ou la cuisson. Cultivés localement.",
                Quantity = "100 kg",
                SellerName = "JEAN",
                SellerLocation = "YAOUNDÉ"
            },
            new Product 
            { 
                Name = "Fruit de tomates", 
                Price = "700F/KG", 
                Image = "tomatoes.jpg",
                Description = "Tomates mûres, cultivée localement sans pesticides. idéales pour les sauces et salades. Récoltés ce matin.",
                Quantity = "50 kg",
                SellerName = "ALEX",
                SellerLocation = "DOUALA"
            },
            new Product 
            { 
                Name = "ANANAS", 
                Price = "200F/KG", 
                Image = "pineapples.jpg",
                Description = "Ananas sucrés et juteux, fraîchement récoltés.",
                Quantity = "200 kg",
                SellerName = "MARIE",
                SellerLocation = "BUEA"
            },
            new Product 
            { 
                Name = "Maïs sec", 
                Price = "350F/KG", 
                Image = "corn.jpg",
                Description = "Maïs sec de bonne qualité pour la consommation ou l'alimentation animale.",
                Quantity = "500 kg",
                SellerName = "PAUL",
                SellerLocation = "BAFOUSSAM"
            }
        };

        FilteredProducts = new ObservableCollection<Product>(Products);

        BindingContext = this;

        Services.CartService.Instance.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Services.CartService.ItemCount))
            {
                OnPropertyChanged(nameof(CartItemCount));
            }
        };
	}

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchTerm = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            FilteredProducts.Clear();
            foreach (var product in Products)
            {
                FilteredProducts.Add(product);
            }
        }
        else
        {
            var filtered = Products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            FilteredProducts.Clear();
            foreach (var product in filtered)
            {
                FilteredProducts.Add(product);
            }
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Product selectedProduct)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedProduct", selectedProduct }
            };
            await Shell.Current.GoToAsync(nameof(ProductDetailsPage), navigationParameter);
            
            // Deselect item
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Menu", "Annuler", null, "Conseils pour acheteurs");
        if (action == "Conseils pour acheteurs")
        {
            await Shell.Current.GoToAsync(nameof(BuyerAdvicePage));
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        // Already on Home (ProductsPage)
    }

    private async void OnChatClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChatListPage));
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
    }
}

public class Product
{
    public string Name { get; set; }
    public string Price { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Quantity { get; set; }
    public string SellerName { get; set; }
    public string SellerLocation { get; set; }

    // Helpers for display
    public string NameAndPrice => $"{Name} {Price}";
    public string QuantityText => $"Quantité disponible : {Quantity}";
    public string SellerNameText => $"Vendu par {SellerName}";
}
