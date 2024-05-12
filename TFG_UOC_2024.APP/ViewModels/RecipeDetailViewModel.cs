using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TFG_UOC_2024.APP.Model;
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
        public string RecipeId
        {
            get => _recipeId;
            set
            {
                if (_recipeId != value)
                {
                    _recipeId = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool _isFavourite;
        public bool IsFavourite
        {
            get => _isFavourite;
            set
            {
                if (_isFavourite != value)
                {
                    _isFavourite = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }

        private Command<object> _isFavouriteCommand;
        public Command<object> IsFavouriteCommand
        {
            get { return _isFavouriteCommand; }
            set { _isFavouriteCommand = value; }
        }

        private Command<object> _backCommand;
        public Command<object> BackCommand
        {
            get { return _backCommand; }
            set { _backCommand = value; }
        }


        private ObservableCollection<IngredientItemModel> _ingredients = new();
        public ObservableCollection<IngredientItemModel> Ingredients
        {
            get => _ingredients;
            set
            {
                if (_ingredients != value)
                {
                    _ingredients = value;
                    OnPropertyChanged();
                }
            }
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
            /*if (RecipeId != null)
            {
                recipe = _recipeService.GetRecipeByIdAsync(Guid.Parse(RecipeId)).Result;
                Description = recipe.Description;
                Name = recipe.Name;
                Image = recipe.ImageUrl;
                IsFavourite = _recipeService.IsFavourite(Guid.Parse(RecipeId), App.user.Id).Result;
                foreach (var item in recipe.IngredientNames.Split(';'))
                {
                    Ingredients.Add(new IngredientItemModel()
                    {
                        Name = item,
                    });
                }
            }*/
        }

        public async void MakeFavourite(object obj)
        {
            if (IsFavourite)
            {
                await _recipeService.DeleteFavourite(Guid.Parse(RecipeId), App.user.Id);
                this.IsFavourite = false;
            }   
            else
            {
                await _recipeService.AddFavourite(Guid.Parse(RecipeId), App.user.Id);
                this.IsFavourite = true;
            }
        }

        public RecipeDetailViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _isFavouriteCommand = new Command<object>(MakeFavourite);
            _backCommand = new Command<object>(GoBack);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RecipeId = query["Id"].ToString();
            recipe = _recipeService.GetRecipeByIdAsync(Guid.Parse(RecipeId)).Result;
            Description = recipe.Description;
            Name = recipe.Name;
            Image = recipe.ImageUrl;
            IsFavourite = _recipeService.IsFavourite(Guid.Parse(RecipeId), App.user.Id).Result;
            foreach (var item in recipe.IngredientNames.Split(';'))
            {
                Ingredients.Add(new IngredientItemModel()
                {
                    Name = item,
                });
            }
        }

        public async void GoBack(object obj)
        {
            await Shell.Current.GoToAsync("..");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
