using Microsoft.Maui.ApplicationModel.Communication;
namespace TFG_UOC_2024.APP.UserControls;

public partial class HeaderControl : ContentView
{
	public HeaderControl()
	{
		InitializeComponent();
        if (App.user != null)
        {
            lblUserName.Text = "Logged in as: " + App.user.Email;
            lblUserEmail.Text = App.user.Email;
        }
    }
}
