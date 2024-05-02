using System.Runtime.CompilerServices;
using Syncfusion.Maui.Scheduler;
using TFG_UOC_2024.APP.ViewModels;

namespace TFG_UOC_2024.APP.Views;

public partial class MenuView : ContentPage
{
    public MenuView(MenuViewModel menuViewModel)
	{
		InitializeComponent();
		BindingContext = menuViewModel;
    }
}