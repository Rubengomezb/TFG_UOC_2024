using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class UserProfileView : ContentPage
{
	public UserProfileView(UserProfileViewModel userProfileViewModel)
	{
		InitializeComponent();
		BindingContext = userProfileViewModel;
	}
}