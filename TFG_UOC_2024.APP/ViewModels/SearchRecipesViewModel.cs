using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.APP.Services;
using CommunityToolkit.Mvvm.Input;
using TFG_UOC_2024.APP.Views;
using Newtonsoft.Json;
using static TFG_UOC_2024.CORE.Components.Authorization.SystemClaims.Permissions;
using System.ComponentModel;
using TFG_UOC_2024.DB.Models;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using TFG_UOC_2024.APP.Model;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class SearchRecipesViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
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

        private Command<object> _generateRecipeCommand;

        public Command<object> GenerateRecipeCommand
        {
            get { return _generateRecipeCommand; }
            set { _generateRecipeCommand = value; }
        }


        private readonly IRecipeService _recipeService;

        private List<string> selectedIngredients;

        private int from = 0;

        private int to = 5;

        public async void OnGenerateAlternatives(object obj)
        {
            from += 5;
            to += 5;
            Recipes = new ObservableCollection<RecipeModel>(GetRecipes().Result);
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
            /*if (App.recipes == null)
            {
                Recipes = new ObservableCollection<RecipeModel>(GetRecipes().Result);
            }
            else
            {
                foreach (var recipe in App.recipes)
                {
                    Recipes.Add(new RecipeModel()
                    {
                        Name = recipe.Name,
                        Description = recipe.Description,
                        ImageUrl = recipe.ImageUrl,
                        Id = recipe.Id.ToString(),
                    });
                }
            }*/
        }

        private async void OnTapped(object obj)
        {
            if (obj is RecipeModel recipe)
            {
                var rec = (recipe as RecipeModel);
                await Shell.Current.GoToAsync($"{nameof(RecipeDetail)}?Id={rec.Id}");
            }
        }

        public SearchRecipesViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _generateRecipeCommand = new Command<object>(OnGenerateAlternatives);
            _tapCommand = new Command<object>(OnTapped);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        { 
            if (_recipes != null && _recipes.Count > 0)
            {
                _recipes.Clear();
            }

            var rand = new Random();   
            selectedIngredients = query["selectedIngredients"].ToString().Split(',').ToList();

            try
            {
                var randomIndex = rand.Next(15);
                from = randomIndex;
                to = from + 5;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (var ingredient in GetRecipes().Result)
            {
                _recipes.Add(new RecipeModel()
                {
                    Description = ingredient.Description,
                    Name = ingredient.Name,
                    Id = ingredient.Id,
                    ImageUrl = ingredient.ImageUrl
                });
            }
        }

        public async Task<List<RecipeModel>> GetRecipes()
        {   var recipesDTO = await _recipeService.GetRecipeByIngredientsAsync(selectedIngredients, from, to).ConfigureAwait(false);
            var recipesModels = new List<RecipeModel>();
            foreach (var recipe in recipesDTO)
            {
                recipesModels.Add(new RecipeModel()
                {
                    Name = recipe.Name,
                    Description = recipe.Description,
                    ImageUrl = recipe.ImageUrl,
                    Id = recipe.Id.ToString(),
                    Ingredients = recipe.IngredientNames.Split(';').ToList(),
                });
            }

            return recipesModels;
        }
    }
}
