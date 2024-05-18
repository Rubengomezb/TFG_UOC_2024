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
        #region Properties
        private readonly IAuthService _authService;
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

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        private Command<object> _logoutCommand;
        public Command<object> LogoutCommand
        {
            get { return _logoutCommand; }
            set { _logoutCommand = value; }
        }

        private Command<object> _updateUserCommand;
        public Command<object> UpdateUserCommand
        {
            get { return _updateUserCommand; }
            set { _updateUserCommand = value; }
        }

        private bool _isInitialized = false;

        public ObservableCollection<UserFoodModel> _userFoodModel = new();
        public ObservableCollection<UserFoodModel> UserFoodModel
        {
            get { return _userFoodModel; }
            set
            {
                _userFoodModel = value;
                RaisePropertyChanged("UserFoodModel");
            }
        }

        private UserFoodModel _selectedItem = new UserFoodModel();
        public UserFoodModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        #endregion

        #region Constructor
        public UserProfileViewModel(IAuthService authService)
        {
            _authService = authService;
            _userFoodModel = new ObservableCollection<UserFoodModel>()
            {
                new UserFoodModel { Id = 1, Name = "Vegetarian" },
                new UserFoodModel { Id = 2, Name = "Vegan" },
                new UserFoodModel { Id = 3, Name = "Mediterranean" },
                new UserFoodModel { Id = 4, Name = "Muslim" },
                new UserFoodModel { Id = 5, Name = "Celiac" },
                new UserFoodModel { Id = 6, Name = "Diet" },
                new UserFoodModel { Id = 7, Name = "Diabetic" },
            };
            _updateUserCommand = new Command<object>(UpdateUser);
            _logoutCommand = new Command<object>(Logout);
        }
        #endregion

        #region Methods
        [RelayCommand]
        async Task AppearingAsync()
        {
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
                var dto = new UserSimpleDTO() { Id = Guid.Parse(Id), FirstName = FirstName, LastName = LastName, PhoneNumber = PhoneNumber, UserName = App.user.UserName, ContactId = App.user.ContactId, Email = App.user.Email, CreatedOn = DateTime.UtcNow, FoodType = this.SelectedItem.Id };

                var simpleUser = await _authService.UpdateUserAsync(_id.ToString(), dto);

                var user = new UserDTO();
                user = App.user;
                user.FirstName = simpleUser.FirstName;
                user.LastName = simpleUser.LastName;
                user.PhoneNumber = simpleUser.PhoneNumber;
                user.Id = simpleUser.Id;
                user.Email = simpleUser.Email;
                user.CreatedOn = simpleUser.CreatedOn;
                user.IsDeleted = simpleUser.IsDeleted;
                user.FoodType = simpleUser.FoodType;

                App.user = user;
                LoadUser();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al registrar el usuario", "Ok");
            }
        }

        public async void Logout(object obj)
        {
            App.user = null;
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public void LoadUser()
        {
            FirstName = App.user.FirstName;
            LastName = App.user.LastName;
            PhoneNumber = App.user.PhoneNumber;
            Id = App.user.Id.ToString();
            Image = "user.png";

            if (_userFoodModel.Count > 0 && App.user.FoodType > 0)
            {
                if (App.user.FoodType != 0)
                {
                    SelectedItem = new UserFoodModel { Id = App.user.FoodType, Name = _userFoodModel.FirstOrDefault(x => x.Id == App.user.FoodType).Name };
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
