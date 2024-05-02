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
        public ObservableCollection<RecipeModel> Recipes { get; set; } = new();


        private readonly IRecipeService _recipeService;

        private List<string> selectedIngredients;

        private int from = 0;

        private int to = 5;

        [RelayCommand]
        public async Task RecipeSelectedCommand(int recipeId)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipeDetail)}?Id={recipeId}");
        }

        [RelayCommand]
        public void GenerateAlternatives()
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
            if (App.recipes == null)
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
            }
        }

        private async void OnTapped(object obj)
        {
            if (obj is RecipeDTO recipe)
            {
                var rec = (recipe as RecipeDTO);
                await Shell.Current.GoToAsync($"{nameof(IngredientsView)}?Id={rec.Id.ToString()}");
            }
        }

        public SearchRecipesViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            selectedIngredients = query["selectedIngredients"].ToString().Split(',').ToList();
            Recipes = new ObservableCollection<RecipeModel>(GetRecipes().Result);
        }

        public async Task<List<RecipeModel>> GetRecipes()
        {   var recipesDTO = await _recipeService.GetRecipeByIngredientsAsync(selectedIngredients, from, to);
            var recipesModels = new List<RecipeModel>();
            foreach (var recipe in recipesDTO)
            {
                recipesModels.Add(new RecipeModel()
                {
                    Name = recipe.Name,
                    Description = recipe.Description,
                    ImageUrl = recipe.ImageUrl,
                    Id = recipe.Id.ToString(),
                });
            }

            return recipesModels;
        }
    }
}
