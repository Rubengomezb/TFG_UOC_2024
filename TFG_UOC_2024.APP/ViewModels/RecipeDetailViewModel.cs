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
        #region Properties
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

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (_isOpen != value)
                {
                    _isOpen = value;
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

        public Command<object> _isOpenCommand;
        public Command<object> IsOpenCommand
        {
            get { return _isOpenCommand; }
            set { _isOpenCommand = value; }

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

        private ObservableCollection<NutricionalInfoModel> _nutritionalInfo = new();
        public ObservableCollection<NutricionalInfoModel> NutritionalInfo
        {
            get => _nutritionalInfo;
            set
            {
                if (_nutritionalInfo != value)
                {
                    _nutritionalInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isInitialized = false;
        #endregion

        #region Constructor
        public RecipeDetailViewModel(IRecipeService recipeService)
        {
            _recipeService = recipeService;
            _isFavouriteCommand = new Command<object>(MakeFavourite);
            _isOpenCommand = new Command<object>(OpenCommand);
        }
        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            RecipeId = query["Id"].ToString();
            LoadRecipeDetail();
        }

        public void LoadRecipeDetail()
        {
            recipe = _recipeService.GetRecipeByIdAsync(Guid.Parse(RecipeId)).Result;
            if (recipe == null) return;
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
            _nutritionalInfo.Add(new NutricionalInfoModel()
            {
                Calories = recipe.Calories,
                Proteins = recipe.Proteins,
                Fats = recipe.Fats,
                Carbohydrates = recipe.Carbohydrates
            });

        }

        private async void OpenCommand(object obj)
        {
            IsOpen = true;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
