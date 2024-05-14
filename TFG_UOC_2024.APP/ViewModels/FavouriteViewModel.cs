using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Model;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class FavouriteViewModel : ObservableObject, INotifyPropertyChanged
    {
        public ObservableCollection<RecipeModel> _recipes { get; set; } = new();

        public ObservableCollection<RecipeModel> Recipes
        {
            get { return _recipes; }
            set
            {
                if (_recipes == value)
                    return;

                _recipes = value;
                OnPropertyChanged();
            }
        }

        private Command<object> _tapCommand;

        public Command<object> TapCommand
        {
            get { return _tapCommand; }
            set { _tapCommand = value; }
        }

        private readonly IRecipeService _recipeService;

        public FavouriteViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            TapCommand = new Command<object>(OnTapped);
        }

        private bool _isInitialized = false;
        [RelayCommand]
        async Task AppearingAsync()
        {
            _isInitialized = true;
            await RefreshAsync();
        }

        [RelayCommand]
        async Task RefreshAsync()
        {
            GetFavouriteRecipes();
        }

        private async void OnTapped(object obj)
        {
            if (obj is RecipeModel recipe)
            {
                var rec = (recipe as RecipeModel);
                await Shell.Current.GoToAsync($"{nameof(RecipeDetail)}?Id={rec.Id}");
            }
        }

        public async void GetFavouriteRecipes()
        {
            if (_recipes != null && _recipes.Any())
            {
                _recipes.Clear();
            }

            var recipesDTO = await _recipeService.GetFavouritesAsync(App.user.Id).ConfigureAwait(false);
            foreach (var recipe in recipesDTO)
            {
                _recipes.Add(new RecipeModel()
                {
                    Id = recipe.Id.ToString(),
                    Name = recipe.Name,
                    Description = recipe.Description,
                    ImageUrl = recipe.ImageUrl,
                    Ingredients = recipe.IngredientNames.Split(';').ToList(),
                });
            }
        }
    }
}
