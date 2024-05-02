using TFG_UOC_2024.APP.Helper;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP
{
    public partial class App : Application
    {
        public static UserDTO user;

        public static List<RecipeDTO> recipes;

        private readonly IAuthService _authService;

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI1Mjc2NkAzMjM1MmUzMDJlMzBqSGR4amQwRUlaVzYvemt2VUh2NlZ6T05kSGpXaFVVdGQ1YXY5QzNsRVlVPQ==");

            InitializeComponent();

            /*if (App.user != null)
            {
                MainPage = new AppShell();
            }
            else
            {
                var authService = ServiceHelper.GetService<IAuthService>();
                var loginviewmodel = new LoginPageViewModel(authService);
                MainPage = new LoginPage(loginviewmodel);
            }*/
            MainPage = new AppShell();
        }
    }
}
