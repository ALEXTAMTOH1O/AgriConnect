using System.Collections.ObjectModel;

namespace AgriConnect;

[QueryProperty(nameof(SellerName), "SellerName")]
public partial class ChatPage : ContentPage
{
    private string _sellerName;
    public string SellerName
    {
        get => _sellerName;
        set
        {
            _sellerName = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ChatMessage> Messages { get; set; }
    public string NewMessageText { get; set; }

	public ChatPage()
	{
		InitializeComponent();
        Messages = new ObservableCollection<ChatMessage>
        {
            new ChatMessage { Text = "Bonjour, ce produit est-il toujours disponible ?", IsIncoming = false },
            new ChatMessage { Text = "Oui, tout à fait !", IsIncoming = true }
        };
        BindingContext = this;
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void OnSendClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(NewMessageText))
        {
            Messages.Add(new ChatMessage { Text = NewMessageText, IsIncoming = false });
            NewMessageText = string.Empty;
            OnPropertyChanged(nameof(NewMessageText));
        }
    }
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(ProductsPage)}");
    }

    private async void OnCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CartPage));
    }
}

public class ChatMessage
{
    public string Text { get; set; }
    public bool IsIncoming { get; set; }

    public Color BackgroundColor => IsIncoming ? Color.FromArgb("#E0E0E0") : Color.FromArgb("#00C853");
    public Color TextColor => IsIncoming ? Colors.Black : Colors.White;
    public LayoutOptions Alignment => IsIncoming ? LayoutOptions.Start : LayoutOptions.End;
}
