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

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class SearchRecipesViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private string _image; 

        public ObservableCollection<RecipeDTO> Recipes { get; set; } = new();


        private readonly IRecipeService _recipeService;

        private List<string> selectedIngredients;

        private int from = 0;

        private int to = 5;

        public string TextSearch
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                OnPropertyChanged(nameof(TextSearch));
                if (_searchText.Length > 0)
                {
                    OnSearchCommand();
                }
                else
                {
                    Recipes = new ObservableCollection<RecipeDTO>(App.recipes);
                }
            }
            
        }

        [RelayCommand]
        public async Task RecipeSelectedCommand(int recipeId)
        {
            await Shell.Current.GoToAsync($"{nameof(RecipeDetail)}?Id={recipeId}");
        }

        [RelayCommand]
        public void GenerateAlternativesCommand()
        {
            from += 5;
            to += 5;
            Recipes = new ObservableCollection<RecipeDTO>(GetRecipes().Result);
            /*string recipeDetails = JsonConvert.SerializeObject(Recipes);
            Preferences.Set(nameof(App.recipes), recipeDetails);
            App.recipes = Recipes;*/
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
                Recipes = new ObservableCollection<RecipeDTO>(GetRecipes().Result);
            }
            else
            {
                Recipes = new ObservableCollection<RecipeDTO>(App.recipes);
            }
        }

        public SearchRecipesViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        private void OnSearchCommand()
        {
            Recipes = new ObservableCollection<RecipeDTO>(App.recipes);
            var foundRecipes = Recipes.Where(r => r.Name.Contains(TextSearch) || r.Description.Contains(TextSearch)).ToList();
            if (foundRecipes.Count > 0)
            {
                Recipes.Clear();
                foreach (var recipe in foundRecipes)
                {
                    Recipes.Add(recipe);
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            selectedIngredients = query["selectedIngredients"].ToString().Split(',').ToList();
            Recipes = new ObservableCollection<RecipeDTO>(GetRecipes().Result);
        }

        public async Task<List<RecipeDTO>> GetRecipes()
        {
            return await _recipeService.GetRecipeByIngredientsAsync(selectedIngredients, from, to);
        }
    }
}
