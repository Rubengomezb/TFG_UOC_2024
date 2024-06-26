﻿using System;
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
using static TFG_UOC_2024.CORE.Components.Authorization.SystemClaims.Permissions;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        public bool forTest = false;

        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public LoginPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Methods
        [RelayCommand]
        public async Task Login()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    var loginDto = new Login();
                    loginDto.Username = UserName;
                    loginDto.Password = Password;

                    var user = await _authService.LoginAsync(loginDto);
                    if (user == null)
                    {
                        await Shell.Current.DisplayAlert("Error", "Username/Password is incorrect", "Ok");
                        return;
                    }

                    App.user = user;

                    if (!forTest)
                    {
                        await Shell.Current.GoToAsync("//MainRecipeView");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "All fields required", "Ok");
                    return;
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return;
            }
        }

        [RelayCommand]
        public async Task SignUp()
        {
            await Shell.Current.GoToAsync($"{nameof(SignUpPage)}");
        }
        #endregion
    }
}
