using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class RecipeDetailViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IRecipeService _recipeService;
        private RecipeDTO recipe;

        [ObservableProperty]
        private string _recipeId;

        [ObservableProperty]
        private bool _isFavourite;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private List<IngredientDTO> _ingredients;

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
            recipe = _recipeService.GetRecipeByIdAsync(Guid.Parse(RecipeId)).Result;
            Description = recipe.Description;
            Name = recipe.Name;
            Ingredients = recipe.Ingredients;
            IsFavourite = _recipeService.IsFavourite(Guid.Parse(RecipeId), App.user.Id).Result;
        }

        [RelayCommand]
        public async Task MakeFavourite(int recipeId)
        {
            if (IsFavourite)
            {
                await _recipeService.DeleteFavourite(Guid.Parse(RecipeId), App.user.Id);
            }   
            else
            {
                await _recipeService.AddFavourite(Guid.Parse(RecipeId), App.user.Id);
            }
        }

        public RecipeDetailViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RecipeId = query["Id"].ToString();
        }

        async Task GoBack()
        {
            await Shell.Current.GoToAsync("../SearchRecipesView");
        }
    }
}
