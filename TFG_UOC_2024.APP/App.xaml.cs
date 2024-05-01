using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP
{
    public partial class App : Application
    {
        public static UserDTO user;

        public static List<RecipeDTO> recipes;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
