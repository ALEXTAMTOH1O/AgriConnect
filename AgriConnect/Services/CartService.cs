using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AgriConnect.Services;

public class CartService : INotifyPropertyChanged
{
    private static CartService _instance;
    public static CartService Instance => _instance ??= new CartService();

    public ObservableCollection<Product> CartItems { get; private set; } = new ObservableCollection<Product>();

    public int ItemCount => CartItems.Count;

    public event PropertyChangedEventHandler PropertyChanged;

    private CartService() { }

    public void AddToCart(Product product)
    {
        CartItems.Add(product);
        OnPropertyChanged(nameof(ItemCount));
    }

    public void RemoveFromCart(Product product)
    {
        CartItems.Remove(product);
        OnPropertyChanged(nameof(ItemCount));
    }

    public void ClearCart()
    {
        CartItems.Clear();
        OnPropertyChanged(nameof(ItemCount));
    }

    public double GetTotal()
    {
        // Assuming Price is in format "700F/KG"
        // This is a simple parser for the mock data
        double total = 0;
        foreach (var item in CartItems)
        {
            var priceString = item.Price.Split('F')[0];
            if (double.TryParse(priceString, out double price))
            {
                total += price;
            }
        }
        return total;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
