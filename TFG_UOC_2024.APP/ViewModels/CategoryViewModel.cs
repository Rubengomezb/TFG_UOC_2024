using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TFG_UOC_2024.APP.Model;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class CategoryViewModel: INotifyPropertyChanged
    {
        #region Properties
        private Command<object> tapCommand;

        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        public ObservableCollection<CategoryModel> _categories { get; set; } = new();

        public ObservableCollection<CategoryModel> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories == value)
                    return;

                _categories = value;
                OnPropertyChanged();
            }
        }

        private readonly IRecipeService _recipeService;

        private bool _isInitialized = false;
        #endregion

        #region Constructor
        public CategoryViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            tapCommand = new Command<object>(OnTapped);
        }
        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;

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
            GetCategories();
        }

        private async void OnTapped(object obj)
        {
            if (obj is CategoryModel category)
            {
                var cat = (obj as CategoryModel);
                await CategorySelectedCommand(cat.Id);
            }
        }

        public async void GetCategories()
        {
            var categoriesDTO = await _recipeService.GetCategories();
            if (categoriesDTO == null) return;
            foreach (var category in categoriesDTO)
            {
                _categories.Add(new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = category.ImageUrl
                });
            }
        }

        public async Task CategorySelectedCommand(string Id)
        {
            await Shell.Current.GoToAsync($"{nameof(IngredientsView)}?Id={Id}");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
