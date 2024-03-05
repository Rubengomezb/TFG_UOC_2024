using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ApplicationDetailsPage), typeof(ApplicationDetailsPage));
        }
    }
}
