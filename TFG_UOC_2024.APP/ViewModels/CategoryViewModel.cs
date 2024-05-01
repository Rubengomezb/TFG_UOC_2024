using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using TFG_UOC_2024.CORE.Models.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        [ObservableProperty]
        public string _image;

        public ObservableCollection<CategoryDTO> Categories { get; set; } = new();

        private readonly IRecipeService _recipeService;

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
            GetCategories();
        }

        public CategoryViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async void GetCategories()
        {
            Categories = new ObservableCollection<CategoryDTO>(await _recipeService.GetCategories());
        }

        public async Task CategorySelectedCommand(int Id)
        {
            // TODO
            await Shell.Current.GoToAsync($"{nameof(IngredientsView)}?Id={Id}");
        }
    }
}
