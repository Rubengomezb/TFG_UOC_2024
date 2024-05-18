using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpViewModel signUpViewModel)
	{
		InitializeComponent();
		BindingContext = signUpViewModel;
	}
}