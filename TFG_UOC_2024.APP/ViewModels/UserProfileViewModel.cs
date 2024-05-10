using System;
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
using TFG_UOC_2024.APP.Model;
using System.ComponentModel;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string _email;

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged("PhoneNumber");
            }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged("Image");
            }
        }

        private string Id;

        private readonly IAuthService _authService;

        private Command<object> _updateUserCommand;

        public Command<object> UpdateUserCommand
        {
            get { return _updateUserCommand; }
            set { _updateUserCommand = value; }
        }

        public UserProfileViewModel(IAuthService authService)
        {
            _authService = authService;
            _userFoodModel = new ObservableCollection<UserFoodModel>()
            {
                new UserFoodModel { Id = 1, Name = "Vegetarian" },
                new UserFoodModel { Id = 2, Name = "Vegan" },
                new UserFoodModel { Id = 3, Name = "Mediterranean" },
                new UserFoodModel { Id = 4, Name = "Muslim" },
                new UserFoodModel { Id = 5, Name = "Jewish" },
                new UserFoodModel { Id = 6, Name = "Diet" },
            };
            _updateUserCommand = new Command<object>(UpdateUser);
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

        public async void UpdateUser(object obj)
        {
            try
            {
                var dto = new UserSimpleDTO() { Id = Guid.Parse(Id), FirstName = FirstName, LastName = LastName, PhoneNumber = PhoneNumber };
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    /*var user = _m.Map<UserDTO>(await _authService.UpdateUserAsync(_id.ToString(), dto));
                    App.user = user;
                    if (Preferences.ContainsKey(nameof(App.user)))
                    {
                        Preferences.Remove(nameof(App.user));
                    }
                    string userDetails = JsonConvert.SerializeObject(user);
                    Preferences.Set(nameof(App.user), userDetails);*/
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

        public ObservableCollection<UserFoodModel> _userFoodModel = new ();
        public ObservableCollection<UserFoodModel> UserFoodModel
        {
            get { return _userFoodModel; }
            set
            {
                _userFoodModel = value;
                RaisePropertyChanged("UserFoodModel");
            }
        }

        private ObservableCollection<UserFoodModel> selectedItem = new ObservableCollection<UserFoodModel>();
        public ObservableCollection<UserFoodModel> SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        private void LoadUser()
        {
            FirstName = App.user.FirstName;
            LastName = App.user.LastName;
            PhoneNumber = App.user.PhoneNumber;
            Email = App.user.Email;
            Id = App.user.Id.ToString();
            Image = "user.png";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
