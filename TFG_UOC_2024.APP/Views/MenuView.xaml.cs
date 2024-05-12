using System.Runtime.CompilerServices;
using Syncfusion.Maui.Scheduler;
using TFG_UOC_2024.APP.ViewModels;
using Syncfusion.Maui.Picker;

namespace TFG_UOC_2024.APP.Views;

public partial class MenuView : ContentPage
{
    public MenuView(MenuViewModel menuViewModel)
	{
		InitializeComponent();
		BindingContext = menuViewModel;
    }

    async void OnButtonClicked(object sender, EventArgs args)
    {

        var viewModel = this.BindingContext as MenuViewModel;
        var _menuService = viewModel.getServiceInstance();
        viewModel.IsOpen = true;
        await _menuService.CreateWeeklyMenuAsync(viewModel.SelectedDate, viewModel.SelectedDate);
        var newRecipes = await _menuService.GetWeeklyMenuAsync(viewModel.SelectedDate, viewModel.SelectedDate);
        foreach (var re in newRecipes)
        {
            viewModel.Events.Add(viewModel.ParseMenuResponse(re));
        }

        var visiblesDates = new List<DateTime>();
        visiblesDates.Add(viewModel.SelectedDate);
        viewModel.QueryAppointments(visiblesDates);
        viewModel.SelectedDateMenus = viewModel.GetSelectedDateAppointments(viewModel.SelectedDate);
        this.appointmentListView.IsVisible = true;
        this.noEventsLabel.IsVisible = false;
    }


}