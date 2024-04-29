namespace TFG_UOC_2024.APP.Views;

public partial class MainRecipeView : ContentPage
{
	public MainRecipeView()
	{
		InitializeComponent();
	}

    private async Task Button_Clicked()
    {
        await Shell.Current.GoToAsync($"{nameof(CategoryView)}");
    }
}