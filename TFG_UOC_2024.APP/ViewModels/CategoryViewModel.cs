using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<CategoryDTO> _categories;

        private readonly IRecipeService _recipeService;
        
        public CategoryViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            GetCategories();
        }

        public async void GetCategories()
        {
            Categories = await _recipeService.GetCategories();
        }

        public async Task CategorySelectedCommand(int Id)
        {
            // TODO
            await Shell.Current.GoToAsync($"{nameof(IngredientsView)}?Id={Id}");
        }
    }
}
