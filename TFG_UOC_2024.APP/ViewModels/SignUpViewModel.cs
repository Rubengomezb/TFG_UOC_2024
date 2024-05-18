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
        #region Properties
        [ObservableProperty]
        private string _username;

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

        [ObservableProperty]
        private string _image;

        [ObservableProperty]
        private string _phoneNumber;

        public bool forTest = false;

        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public SignUpViewModel(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Methods
        [RelayCommand]
        public async Task SignUp()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Username) &&
                    !string.IsNullOrWhiteSpace(Password) &&
                    !string.IsNullOrWhiteSpace(Email) &&
                    !string.IsNullOrWhiteSpace(FirstName) &&
                    !string.IsNullOrWhiteSpace(LastName) &&
                    !string.IsNullOrWhiteSpace(PhoneNumber))
                {
                    var signUpDto = new UserInput();
                    signUpDto.UserName = Username;
                    signUpDto.Password = Password;
                    signUpDto.Email = Email;
                    signUpDto.FirstName = FirstName;
                    signUpDto.LastName = LastName;
                    signUpDto.PhoneNumber = PhoneNumber;

                    var user = await _authService.SignUpAsync(signUpDto);
                    if (user == Guid.Empty)
                    {
                        await Shell.Current.DisplayAlert("Error", "Username/Password is incorrect", "Ok");
                        return;
                    }

                    var loginDto = new Login();
                    loginDto.Username = Username;
                    loginDto.Password = Password;

                    var userLogged = await _authService.LoginAsync(loginDto);
                    if (userLogged != null)
                    {
                        //if (Preferences.ContainsKey(nameof(App.user)))
                        //{
                        //    Preferences.Remove(nameof(App.user));
                        //}
                        //string userDetails = JsonConvert.SerializeObject(user);
                        //Preferences.Set(nameof(App.user), userDetails);
                        App.user = userLogged;
                        //AppShell.Current.FlyoutHeader = new HeaderControl();
                        if (!forTest)
                        {
                            await Shell.Current.GoToAsync("//MainRecipeView");
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all the fields", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        [RelayCommand]
        public async Task Login()
        {
            await Shell.Current.GoToAsync("..");
        }
        #endregion
    }
}
