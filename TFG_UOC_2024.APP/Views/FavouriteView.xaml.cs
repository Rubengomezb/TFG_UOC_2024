using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class FavouriteView : ContentPage
{
	public FavouriteView(FavouriteViewModel favouriteViewModel)
	{
		InitializeComponent();
        BindingContext = favouriteViewModel;
    }
}