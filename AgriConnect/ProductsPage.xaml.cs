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
            },
            new Product 
            { 
                Name = "Cacao en vrac", 
                Price = "1500F/KG", 
                Image = "cocoa_farm.jpg",
                Description = "Fèves de cacao fraîches, récoltées directement de la ferme. Idéal pour la transformation ou l'exportation.",
                Quantity = "1000 kg",
                SellerName = "PIERRE",
                SellerLocation = "EBOLOWA"
            },
            new Product 
            { 
                Name = "Chocolat Artisanal", 
                Price = "5000F/Tab", 
                Image = "cocoa_beans.jpg",
                Description = "Chocolat noir artisanal fait à partir de nos meilleures fèves. Vendu avec un échantillon de fèves séchées.",
                Quantity = "50 unités",
                SellerName = "SOPHIE",
                SellerLocation = "KRIBI"
            },
            new Product 
            { 
                Name = "Café Vital Moulu", 
                Price = "2500F/Paq", 
                Image = "coffee_vital.jpg",
                Description = "Café moulu pur Arabica du Cameroun. Arôme riche et goût intense. Paquet de 500g.",
                Quantity = "200 paquets",
                SellerName = "COOPÉRATIVE OKU",
                SellerLocation = "BAMENDA"
            },
            new Product 
            { 
                Name = "Riz de Ndop", 
                Price = "450F/KG", 
                Image = "rice_grains.jpg",
                Description = "Riz local de Ndop, grains longs et parfumés. Nettoyé et prêt à cuire.",
                Quantity = "5000 kg",
                SellerName = "AHMADOU",
                SellerLocation = "NDOP"
            },
            new Product 
            { 
                Name = "Oignons Rouges", 
                Price = "600F/KG", 
                Image = "onions.png",
                Description = "Oignons rouges fermes et savoureux, parfaits pour la cuisine quotidienne. Conservation longue durée.",
                Quantity = "300 kg",
                SellerName = "FATIMATOU",
                SellerLocation = "MAROUA"
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

    private async void OnProductTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Product selectedProduct)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", selectedProduct }
            };
            await Shell.Current.GoToAsync(nameof(ProductDetailsPage), navigationParameter);
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

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
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
