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
        }
    }
}
