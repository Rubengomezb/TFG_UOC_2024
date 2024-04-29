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

namespace TFG_UOC_2024.APP.ViewModels
{
    public partial class IngredientsViewModel : ObservableObject, IQueryAttributable, INotifyPropertyChanged
    {
        [ObservableProperty]
        private string _categoryId;

        [ObservableProperty]
        private List<IngredientDTO> _ingredients;

        private List<IngredientDTO> SelectedIngredients;

        private readonly IRecipeService _recipeService;

        public IngredientsViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            SelectedIngredients = new List<IngredientDTO>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CategoryId = query["Id"].ToString();
            Ingredients = _recipeService.GetIngredients(_categoryId).Result;
        }

        public void SelectIngredientCommand(int Id)
        {
            SelectedIngredients.Add(Ingredients.Find(x => x.Name == Id.ToString()));
        }

        public async Task GenerateRecipe()
        {
            var selectIngredientsNames = string.Join(",", SelectedIngredients.Select(x => x.Name).ToList());
            await Shell.Current.GoToAsync($"{nameof(SearchRecipesView)}?selectedIngredients={selectIngredientsNames}");
        }
    }
}
