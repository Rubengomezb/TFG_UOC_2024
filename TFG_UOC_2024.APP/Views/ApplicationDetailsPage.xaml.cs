using TFG_UOC_2024.APP.Services;

namespace TFG_UOC_2024.APP.Views;

public partial class ApplicationDetailsPage : ContentPage
{
    private readonly IAuthService _authService;

    public ApplicationDetailsPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        _authService.Logout();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}