using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.UserControls;
using TFG_UOC_2024.APP.Views;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Networking;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _confirmPassword;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        private readonly IAuthService _authService;

        public SignUpViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task SignUp()
        {
            try
            {
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                    {
                        var signUpDto = new UserInput();
                        signUpDto.UserName = UserName;
                        signUpDto.Password = Password;
                        signUpDto.Email = Email;
                        signUpDto.FirstName = FirstName;
                        signUpDto.LastName = LastName;

                        var user = await _authService.SignUpAsync(signUpDto);
                        if (user == Guid.Empty)
                        {
                            await Shell.Current.DisplayAlert("Error", "Username/Password is incorrect", "Ok");
                            return;
                        }

                        var loginDto = new Login();
                        loginDto.Username = UserName;
                        loginDto.Password = Password;

                        var userLogged = await _authService.LoginAsync(loginDto);
                        if (userLogged != null)
                        {
                            if (Preferences.ContainsKey(nameof(App.user)))
                            {
                                Preferences.Remove(nameof(App.user));
                            }
                            string userDetails = JsonConvert.SerializeObject(user);
                            Preferences.Set(nameof(App.user), userDetails);
                            App.user = userLogged;
                            AppShell.Current.FlyoutHeader = new HeaderControl();
                            await Shell.Current.GoToAsync($"{nameof(MainRecipeView)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
