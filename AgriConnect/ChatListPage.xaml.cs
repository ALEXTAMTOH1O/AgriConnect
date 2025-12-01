using System.Collections.ObjectModel;

namespace AgriConnect;

public partial class ChatListPage : ContentPage
{
    public ObservableCollection<Conversation> Conversations { get; set; }

    public ChatListPage()
    {
        InitializeComponent();

        Conversations = new ObservableCollection<Conversation>
        {
            new Conversation { SellerName = "JEAN", LastMessage = "Bonjour, les plantains sont toujours disponibles ?" },
            new Conversation { SellerName = "ALEX", LastMessage = "Oui, je peux vous livrer demain." },
            new Conversation { SellerName = "MARIE", LastMessage = "Le prix est négociable pour 50kg." }
        };

        BindingContext = this;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Conversation conversation)
        {
            // Navigate to ChatPage
            await Shell.Current.GoToAsync(nameof(ChatPage));
            
            // Deselect item
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(ProductsPage)}");
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

public class Conversation
{
    public string SellerName { get; set; }
    public string LastMessage { get; set; }
}
