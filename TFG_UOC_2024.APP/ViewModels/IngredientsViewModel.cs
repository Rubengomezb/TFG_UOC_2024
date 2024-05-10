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
using System.Runtime.CompilerServices;

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class IngredientsViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {

        private string CategoryId;

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
            //GetCategories();
        }

        /*private Command<object> _chipAction;

        public Command<object> ChipAction
        {
            get { return _chipAction; }
            set { _chipAction = value; }
        }*/

        public ObservableCollection<IngredientModel> _ingredients { get; set; } = new();

        public ObservableCollection<IngredientModel> Ingredients
        {
            get { return _ingredients; }
            set
            {
                if (_ingredients == value)
                    return;

                _ingredients = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<SelectedIngredientModel> _selectedIngredients { get; set; } = new();

        public ObservableCollection<SelectedIngredientModel> SelectedIngredients
        {
            get { return _selectedIngredients; }
            set
            {
                if (_selectedIngredients == value)
                    return;

                _selectedIngredients = value;
                OnPropertyChanged();
            }
        }

        private readonly IRecipeService _recipeService;

        public IngredientsViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _generateRecipeCommand = new Command<object>(OnGenerateRecipe);
            //_chipAction = new Command<object>(OnRemoveChip);
            _tapCommand = new Command<object>(OnTapped);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async void OnTapped(object obj)
        {
            if (obj is IngredientModel ingredient)
            {
                var ing = (obj as IngredientModel);
                SelectIngredientCommand(ing.Name, true);
            }
        }

        private async void OnRemoveChip(object obj)
        {
            if (obj is SelectedIngredientModel ingredient)
            {
                var ing = (obj as SelectedIngredientModel);
                SelectIngredientCommand(ing.Name, false);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _ingredients.Clear();
            CategoryId = query["Id"].ToString();
            LoadIngredients();
        }

        public void SelectIngredientCommand(string name, bool isAdd = true)
        {
            if (!SelectedIngredients.Any(x => x.Name == name))
                SelectedIngredients.Add(new SelectedIngredientModel { Name = name });
        }

        public async void OnGenerateRecipe(object obj)
        {
            var selectIngredientsNames = string.Join(",", SelectedIngredients.Select(x => x.Name).ToList());
            await Shell.Current.GoToAsync($"{nameof(SearchRecipesView)}?selectedIngredients={selectIngredientsNames}");
        }
        
        private void LoadIngredients()
        {
            var ingredientDTOs = _recipeService.GetIngredients(CategoryId).Result;

            foreach (var ingredient in ingredientDTOs)
            {
                _ingredients.Add(new IngredientModel
                {
                    Name = ingredient.Name,
                    ImageUrl = ingredient.ImageUrl
                });
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
