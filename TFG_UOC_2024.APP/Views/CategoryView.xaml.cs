using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class CategoryView : ContentPage
{
	public CategoryView(CategoryViewModel categoryViewModel)
	{
		InitializeComponent();
		BindingContext = categoryViewModel;
	}
}