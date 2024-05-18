using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthService _authService;

        public MainPage(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            
        }
    }

}
