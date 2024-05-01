using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class SearchRecipesView : ContentPage
{
	public SearchRecipesView(SearchRecipesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}