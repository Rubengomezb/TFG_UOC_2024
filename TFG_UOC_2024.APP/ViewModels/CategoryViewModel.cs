using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private Command<object> tapCommand;

        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        public ObservableCollection<CategoryModel> Categories { get; set; } = new();

        private readonly IRecipeService _recipeService;

        private bool _isInitialized = false;

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

        public CategoryViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            tapCommand = new Command<object>(OnTapped);
        }

        private async void OnTapped(object obj)
        {
            if (obj is CategoryModel category)
            {
                var cat = (obj as CategoryModel);
                await CategorySelectedCommand(int.Parse(cat.Id));
            }
        }

        public async void GetCategories()
        {
            Categories = new ObservableCollection<CategoryModel>();
            var categoriesDTO = await _recipeService.GetCategories();
            foreach (var category in categoriesDTO)
            {
                Categories.Add(new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = category.ImageUrl
                });
            } 
        }

        public async Task CategorySelectedCommand(int Id)
        {
            await Shell.Current.GoToAsync($"{nameof(IngredientsView)}?Id={Id}");
        }
    }
}
