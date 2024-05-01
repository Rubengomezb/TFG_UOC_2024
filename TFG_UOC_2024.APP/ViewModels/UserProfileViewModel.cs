﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.CORE.Models.DTOs;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private string _id;

        private readonly IAuthService _authService;

        private readonly IMapper _m;

        public UserProfileViewModel(IAuthService authService, IMapper m)
        {
            _authService = authService;
            _m = m;
        }

        private bool _isInitialized = false;
        [RelayCommand]
        async Task AppearingAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            await RefreshAsync();
        }

        [RelayCommand]
        async Task RefreshAsync()
        {
            LoadUser();
        }

        [RelayCommand]
        public async Task UpdateUser()
        {
            try
            {
                var dto = new UserSimpleDTO() { Id = Guid.Parse(Id), FirstName = FirstName, LastName = LastName, PhoneNumber = PhoneNumber, Email = Email };
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    var user = _m.Map<UserDTO>(await _authService.UpdateUserAsync(_id.ToString(), dto));
                    App.user = user;
                    if (Preferences.ContainsKey(nameof(App.user)))
                    {
                        Preferences.Remove(nameof(App.user));
                    }
                    string userDetails = JsonConvert.SerializeObject(user);
                    Preferences.Set(nameof(App.user), userDetails);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "No hay conexión a internet", "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al registrar el usuario", "Ok");
            }
        }
        
        private void LoadUser()
        {
            FirstName = App.user.FirstName;
            LastName = App.user.LastName;
            PhoneNumber = App.user.PhoneNumber;
            Email = App.user.Email;
            Id = App.user.Id.ToString();
        }
    }
}
