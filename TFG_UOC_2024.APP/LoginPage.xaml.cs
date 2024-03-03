using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.APP;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage(IAuthService authService)
	{
		InitializeComponent();
        _authService = authService;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var loginDto = new Login();
        loginDto.Username = "admin";
        loginDto.Username = "passW0rd!";
        var error = await _authService.LoginAsync(loginDto);
        if (string.IsNullOrWhiteSpace(error))
        {
            await Shell.Current.GoToAsync($"//{nameof(ApplicationDetailsPage)}");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", error, "Ok");
        }
    }
}