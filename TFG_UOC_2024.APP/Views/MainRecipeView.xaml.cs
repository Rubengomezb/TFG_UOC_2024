namespace TFG_UOC_2024.APP.Views;

public partial class MainRecipeView : ContentPage
{
	public MainRecipeView()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(CategoryView)}");
    }
}