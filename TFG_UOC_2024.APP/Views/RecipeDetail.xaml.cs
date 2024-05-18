using System.Xml.Linq;
using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class RecipeDetail : ContentPage
{
	public RecipeDetail(RecipeDetailViewModel recipeDetailViewModel)
	{
		InitializeComponent();
		BindingContext = recipeDetailViewModel;
	}
}