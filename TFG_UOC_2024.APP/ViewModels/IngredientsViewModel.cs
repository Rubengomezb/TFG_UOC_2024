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

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class IngredientsViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string _categoryId;

        public ObservableCollection<IngredientDTO> Ingredients { get; set; } = new();
        private List<IngredientDTO> SelectedIngredients;

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CategoryId = query["Id"].ToString();
            LoadIngredients();
        }

        public void SelectIngredientCommand(int Id)
        {
            SelectedIngredients.Add(Ingredients.FirstOrDefault(x => x.Name == Id.ToString()));
        }

        public async Task GenerateRecipe()
        {
            var selectIngredientsNames = string.Join(",", SelectedIngredients.Select(x => x.Name).ToList());
            await Shell.Current.GoToAsync($"{nameof(SearchRecipesView)}?selectedIngredients={selectIngredientsNames}");
        }
        
        private void LoadIngredients()
        {
            Ingredients = new ObservableCollection<IngredientDTO>(_recipeService.GetIngredients(CategoryId).Result);
        }
    }
}
