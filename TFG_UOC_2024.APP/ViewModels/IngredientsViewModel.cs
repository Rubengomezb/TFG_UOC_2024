using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TFG_UOC_2024.APP.Model;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class IngredientsViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {

        private string CategoryId;

        private Command<object> tapCommand;

        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        public ObservableCollection<IngredientModel> Ingredients { get; set; } = new();

        private List<IngredientModel> SelectedIngredients;

        private readonly IRecipeService _recipeService;

        public IngredientsViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
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
            LoadIngredients();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void OnTapped(object obj)
        {
            if (obj is IngredientDTO ingredient)
            {
                var ing = (obj as IngredientDTO);
                SelectIngredientCommand(ing.Name);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CategoryId = query["Id"].ToString();
            LoadIngredients();
        }

        public void SelectIngredientCommand(string name)
        {
            SelectedIngredients.Add(Ingredients.FirstOrDefault(x => x.Name == name));
        }

        public async Task GenerateRecipe()
        {
            var selectIngredientsNames = string.Join(",", SelectedIngredients.Select(x => x.Name).ToList());
            await Shell.Current.GoToAsync($"{nameof(SearchRecipesView)}?selectedIngredients={selectIngredientsNames}");
        }
        
        private void LoadIngredients()
        {
            var ingredientDTOs = _recipeService.GetIngredients(CategoryId).Result;

            foreach (var ingredient in ingredientDTOs)
            {
                Ingredients.Add(new IngredientModel
                {
                    Name = ingredient.Name,
                    ImageUrl = ingredient.ImageUrl
                });
            }
        }
    }
}
