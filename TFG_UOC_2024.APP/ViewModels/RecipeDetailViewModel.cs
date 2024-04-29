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

        private string _recipeId;

        [ObservableProperty]
        private bool _isFavourite;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private List<IngredientDTO> _ingredients;

        [RelayCommand]
        public async Task MakeFavourite(int recipeId)
        {
            if (IsFavourite)
            {
                await _recipeService.DeleteFavourite(Guid.Parse(_recipeId), App.user.Id);
            }   
            else
            {
                await _recipeService.AddFavourite(Guid.Parse(_recipeId), App.user.Id);
            }
        }

        public RecipeDetailViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            IsFavourite = _recipeService.IsFavourite(Guid.Parse(_recipeId), App.user.Id).Result;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _recipeId = query["Id"].ToString();
            recipe = _recipeService.GetRecipeByIdAsync(Guid.Parse(_recipeId)).Result;
            Description = recipe.Description;
            Name = recipe.Name;
            Ingredients = recipe.Ingredients;
        }

        async Task GoBack()
        {
            await Shell.Current.GoToAsync("../SearchRecipesView");
        }
    }
}
