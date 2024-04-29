using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class IngredientsView : ContentPage
{
	public IngredientsView(IngredientsViewModel ingredientsViewModel)
	{
		InitializeComponent();
		BindingContext = ingredientsViewModel;
	}
}