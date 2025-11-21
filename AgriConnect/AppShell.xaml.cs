namespace AgriConnect
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register route if RoleSelectionPage type exists       
            var pageType = Type.GetType("AgriConnect.RoleSelectionPage, AgriConnect");      
            if (pageType != null)
            {
                Routing.RegisterRoute("roleselection", pageType);    
            }

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(ChatListPage), typeof(ChatListPage));
            Routing.RegisterRoute(nameof(BuyerAdvicePage), typeof(BuyerAdvicePage));
        }
    }
}
